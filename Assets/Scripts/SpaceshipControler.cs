using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceshipControler : MonoBehaviour {
	protected Spaceship controled;
	protected Spaceship Controled {
		get { 
			if (this.controled == null) {
				this.controled = this.GetComponent<Spaceship> ();
			}
			return this.controled;
		}
	}

	public abstract void InputControl (out float forwardInput, out float rightInput);
}

