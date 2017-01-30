using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceship : Spaceship
{
	public float CloseDistance;
	public float LongDistance;
	private Spaceship Target;

	override protected void OnStart ()
	{
		this.FindTarget ();
	}
	
	private void FindTarget()
	{
		Target = FindObjectOfType<PlayerSpaceship>();
	}

	override protected void InputControl (out float forwardInput, out float rightInput)
	{
		float distance = (Target.transform.position - this.transform.position).magnitude;
		if (distance < - this.CloseDistance)
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

		float angle = Vector3.Angle(this.transform.forward, Target.transform.position - this.transform.position);
		Vector3 cross = Vector3.Cross(this.transform.forward, Target.transform.position - this.transform.position);
		if (cross.y < 0f)
		{
			angle = -angle;
		}
		rightInput = angle / 90f;
		rightInput = Mathf.Min(rightInput, 1f);
		rightInput = Mathf.Max(rightInput, -1f);

		if (Mathf.Abs(angle) < 2f)
		{
			this.Shoot();
		}
	}	
}
