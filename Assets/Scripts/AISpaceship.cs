using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceship : Spaceship
{
	static private float ControlTime = 0f;
	static public float S_ControlTime
	{
		get 
		{ 
			return AISpaceship.ControlTime;
		}
	}

	public enum AIMode {
		None,
		Track,
        Escape,
		Dodge
	};

    // Dodge Obstacle.
    private float dodgeSign = 1f;
    [HideInInspector]
    public float dodgeForwardInput;
    [HideInInspector]
    public float dodgeRightInputCoef;
    [HideInInspector]
    public float dodgeSphereCastRadius;
    [HideInInspector]
    public float dodgeSphereCastDistance;

    // Escape
    [HideInInspector]
    public float escapeForwardInput;
    [HideInInspector]
    public float escapeRightInputCoef;

    // Track
    [HideInInspector]
    public float trackLongForwardInput;
    [HideInInspector]
    public float trackLongRightInputCoef;
    [HideInInspector]
    public float trackShortDistance;
    [HideInInspector]
    public float trackLongDistance;
    [HideInInspector]
    public float trackMiddleRightInputCoef;
    private float trackACoef;
    private float trackBCoef;

	private Spaceship Target;
	private AIMode currentAIMode = AIMode.None;
	public AIMode CurrentAIMode {
		get { 
			return this.currentAIMode;
		}
		private set {
			if (value != this.currentAIMode) {
				//Log.Info (this.name, "Goes into " + value + " mode.");
			}
			this.currentAIMode = value;
		}
	}

	override protected void OnStart ()
	{
        trackACoef = 1f / (this.trackLongDistance - this.trackShortDistance);
        trackBCoef = 1f - (this.trackLongDistance + this.trackShortDistance) / (this.trackLongDistance - this.trackShortDistance) / 2f;
        //Log.BeforeApplicationQuit += this.LogStats;
	}

    protected override void OnUpdate()
    {

    }

    private void FindTarget()
	{
        float minDist = float.MaxValue;
        Spaceship[] spaceships = FindObjectsOfType<Spaceship>();
        foreach (Spaceship spaceship in spaceships)
        {
            if (this.SpaceShipTeam != spaceship.SpaceShipTeam)
            {
                float dist = (this.transform.position - spaceship.transform.position).sqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    this.Target = spaceship;
                }
            }
        }
	}

	override protected void InputControl ()
    {
        this.FindTarget();
        float t1 = Time.realtimeSinceStartup;

        float obstacleAngle;
        bool incomingObstacle = this.IncomingObstacle(out obstacleAngle);
        // If there is an incoming obstacle.
        if (incomingObstacle)
        {
            if (this.CurrentAIMode != AIMode.Dodge)
            {
                this.dodgeSign = Mathf.Sign(obstacleAngle);
            }
            this.CurrentAIMode = AIMode.Dodge;
            forwardInput = this.dodgeForwardInput;
            rightInput = this.dodgeSign * (1f - Mathf.Abs(obstacleAngle) / 90f) * this.dodgeRightInputCoef;
            return;
        }
        // If there is a potential target.
        if (this.Target != null)
        {
            this.CurrentAIMode = AIMode.Track;
            float distance = (Target.transform.position - this.transform.position).magnitude;
            float targetAngle;
            this.TrackTarget(out targetAngle);
            // If target is too close
            if (distance < this.trackShortDistance)
            {
                this.CurrentAIMode = AIMode.Escape;
                forwardInput = this.escapeForwardInput;
                rightInput = Mathf.Sign(targetAngle) * (1f - Mathf.Abs(targetAngle) / 180f) * this.escapeRightInputCoef;
            }
            // If target is far
            else
            {
                this.CurrentAIMode = AIMode.Track;
                if (distance > this.trackLongDistance)
                {
                    forwardInput = this.trackLongForwardInput;
                    rightInput = - (targetAngle / 180f) * 2f * this.trackLongRightInputCoef;
                }
                else
                {
                    forwardInput = this.trackACoef * distance + this.trackBCoef;
                    rightInput = - (targetAngle / 180f) * 2f * this.trackMiddleRightInputCoef;
                }
            }
            if (Mathf.Abs(targetAngle) < 1f)
            {
                if (this.Shoot())
                {
                    this.stats.AddShot();
                    this.stats.AddShot();
                }
            }
        }
        else
        {
            this.FindTarget();
            forwardInput = 0f;
            rightInput = 0f;
        }
        
		AISpaceship.ControlTime += (Time.realtimeSinceStartup - t1);
	}

    /// <summary>
    /// Detects an incoming obstacle.
    /// </summary>
    /// <param name="angle">Incoming obstacle angle will be written here</param>
    /// <returns>true if there is an incoming obstacle</returns>
    private bool IncomingObstacle(out float angle)
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hitInfo;
        Physics.SphereCast(ray, this.dodgeSphereCastRadius, out hitInfo, this.dodgeSphereCastDistance);
        // In case the obstacle is already colliding, another SphereCast will make it seeable.
        if (hitInfo.collider == null)
        {
            Physics.SphereCast(ray, this.dodgeSphereCastRadius / 2f, out hitInfo, this.dodgeSphereCastDistance);
        }
        if (hitInfo.collider != null)
        {
            Obstacle obstacle = hitInfo.collider.GetComponent<Obstacle>();
            // In case an obstacle is spotted in front of the vehicle.
            if (obstacle != null)
            {
                angle = Vector3.Angle(this.transform.forward, obstacle.gameObject.transform.position - this.transform.position);
                Vector3 cross = Vector3.Cross(this.transform.forward, obstacle.gameObject.transform.position - this.transform.position);
                if (cross.y > 0f)
                {
                    angle = -angle;
                }
                return true;
            }
        }
        angle = 0f;
        return false;
    }

    /// <summary>
    /// Localize the angular position of the target.
    /// </summary>
    /// <param name="angle">Target angle will be written here</param>
	private void TrackTarget(out float angle) {
		angle = Vector3.Angle(this.transform.forward, Target.transform.position - this.transform.position);
		Vector3 cross = Vector3.Cross(this.transform.forward, Target.transform.position - this.transform.position);
		if (cross.y > 0f)
		{
			angle = -angle;
		}
	}

	public void OnCollisionEnter(Collision collision){
		Obstacle obstacle = collision.gameObject.GetComponent<Obstacle> ();
		if (obstacle != null) {
			this.stats.AddCollision ();
			//Log.Info (this.name, "Has hit obstacle");
		}
	}
}
