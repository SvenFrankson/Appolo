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

	public float CloseDistance;
	public float LongDistance;
	private Spaceship Target;
	private AIMode currentAIMode = AIMode.None;
	public AIMode CurrentAIMode {
		get { 
			return this.currentAIMode;
		}
		private set {
			if (value != this.currentAIMode) {
				Log.Info (this.name, "Goes into " + value + " mode.");
			}
			this.currentAIMode = value;
		}
	}

	override protected void OnStart ()
	{
		Log.BeforeApplicationQuit += this.LogStats;
	}

    protected override void OnUpdate()
    {
        this.FindTarget();
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

	override protected void InputControl (out float forwardInput, out float rightInput)
    {
        float t1 = Time.realtimeSinceStartup;

        float obstacleAngle;
        bool incomingObstacle = this.IncomingObstacle(out obstacleAngle);
        // If there is an incoming obstacle.
        if (incomingObstacle)
        {
            this.CurrentAIMode = AIMode.Dodge;
            forwardInput = 0.5f;
            rightInput = Mathf.Sign(obstacleAngle) * (1f - obstacleAngle / 180f);
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
            if (distance < this.CloseDistance)
            {
                this.CurrentAIMode = AIMode.Escape;
                rightInput = Mathf.Sign(targetAngle) * (1f - targetAngle / 180f);
                forwardInput = 1f;
            }
            // If target is far
            else
            {
                this.CurrentAIMode = AIMode.Track;
                if (distance > this.LongDistance)
                {
                    rightInput = - (targetAngle / 180f) * 2f;
                    forwardInput = 1f;
                }
                else
                {
                    rightInput = - (targetAngle / 180f) * 2f;
                    float a = 1f / (this.LongDistance - this.CloseDistance);
                    float b = 1f - (this.LongDistance + this.CloseDistance) / (this.LongDistance - this.CloseDistance) / 2f;
                    forwardInput = a * distance + b;
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
        Ray ray = new Ray(this.transform.position, this.C_Rigidbody.velocity);
        RaycastHit hitInfo;
        Physics.SphereCast(ray, 4f, out hitInfo, 20f);
        if (hitInfo.collider != null)
        {
            Obstacle obstacle = hitInfo.collider.GetComponent<Obstacle>();
            // In case an obstacle is spotted in front of the vehicle.
            if (obstacle != null)
            {
                angle = Vector3.Angle(this.transform.forward, obstacle.transform.position - this.transform.position);
                Vector3 cross = Vector3.Cross(this.transform.forward, obstacle.transform.position - this.transform.position);
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
			Log.Info (this.name, "Has hit obstacle");
		}
	}
}
