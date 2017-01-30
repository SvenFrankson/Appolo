using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceship : Spaceship 
{
	public float DistanceMax;
	public float AngleMin;
	public float AngleMax;

	public void OnGUI()
	{
		//GUILayout.TextArea (this.HitPoint + " / " + this.Stamina);
	}

	protected override void OnUpdate ()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			this.Shoot();
		}
	}

	override protected void InputControl (out float forwardInput, out float rightInput)
	{
		forwardInput = TouchControler.TouchValue.y;
		rightInput = TouchControler.TouchValue.x;
	}
}
