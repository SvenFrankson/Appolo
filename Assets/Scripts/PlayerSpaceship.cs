using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpaceship : SpaceshipControler 
{
	public float DistanceMax;
	public float AngleMin;
	public float AngleMax;

	public void OnGUI()
	{
		//GUILayout.TextArea (this.HitPoint + " / " + this.Stamina);
	}

	public void Update ()
	{
		if (TouchControler.Shoot)
		{
			Controled.Shoot();
		}
	}

	public override void InputControl (out float forwardInput, out float rightInput)
	{
		forwardInput = TouchControler.TouchValue.y;
		rightInput = TouchControler.TouchValue.x;
	}
}
