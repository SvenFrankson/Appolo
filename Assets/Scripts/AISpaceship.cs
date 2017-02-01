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
		Dodge
	}

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
		this.FindTarget ();
	}
	
	private void FindTarget()
	{
		Target = FindObjectOfType<PlayerSpaceship>();
	}

	override protected void InputControl (out float forwardInput, out float rightInput)
	{
		float t1 = Time.realtimeSinceStartup;
		float distance = (Target.transform.position - this.transform.position).magnitude;
		if (distance < this.CloseDistance)
		{
			forwardInput = 0f;
		}
		else if (distance > this.LongDistance)
		{
			forwardInput = 1f;
		}
		else
		{
			float a = 1f / (this.LongDistance - this.CloseDistance);
			float b = 1 - (this.LongDistance + this.CloseDistance) / (this.LongDistance - this.CloseDistance) / 2f;
			forwardInput = a * distance + b;
		}

		rightInput = IncommingObstacle ();
		if (rightInput == 0f) {
			rightInput = TrackTarget ();
		}

		AISpaceship.ControlTime += (Time.realtimeSinceStartup - t1);
	}

	private float IncommingObstacle() {
		Ray ray = new Ray (this.transform.position, this.C_Rigidbody.velocity);
		RaycastHit hitInfo;
		Physics.SphereCast (ray, 4f, out hitInfo, 20f);
		if (hitInfo.collider != null) {
			Obstacle obstacle = hitInfo.collider.GetComponent<Obstacle> ();
			// In case an obstacle is spotted in front of the vehicle.
			if (obstacle != null) {
				float angle = Vector3.Angle(this.transform.forward, obstacle.transform.position - this.transform.position);
				Vector3 cross = Vector3.Cross(this.transform.forward, obstacle.transform.position - this.transform.position);
				if (cross.y < 0f)
				{
					angle = -angle;
				}
				CurrentAIMode = AIMode.Dodge;
				return -angle / 90f;
			}
		}
		return 0f;
	}

	private float TrackTarget() {
		float angle = Vector3.Angle(this.transform.forward, Target.transform.position - this.transform.position);
		Vector3 cross = Vector3.Cross(this.transform.forward, Target.transform.position - this.transform.position);
		if (cross.y < 0f)
		{
			angle = -angle;
		}
		float rightInput = angle / 90f;
		rightInput = Mathf.Min(rightInput, 1f);
		rightInput = Mathf.Max(rightInput, -1f);

		if (Mathf.Abs(angle) < 2f)
		{
			bool canShoot = this.Shoot ();
			if (canShoot) {
				this.stats.AddShot ();
				this.stats.AddShot ();
			}
		}
		CurrentAIMode = AIMode.Track;
		return rightInput;
	}

	public void OnCollisionEnter(Collision collision){
		Obstacle obstacle = collision.gameObject.GetComponent<Obstacle> ();
		if (obstacle != null) {
			this.stats.AddCollision ();
			Log.Info (this.name, "Has hit obstacle");
		}
	}
}
