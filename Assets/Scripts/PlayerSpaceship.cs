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
		if (TouchControler.Shoot)
		{
			this.Shoot();
		}
	}

	override protected void InputControl ()
	{
		forwardInput = TouchControler.TouchValue.y;
		rightInput = TouchControler.TouchValue.x;
	}
}
