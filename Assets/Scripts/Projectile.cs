using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float Power;
	public float Lifetime;
	private float Delay = 0f;

	void Update () {
		this.Delay += Time.deltaTime;
		if (this.Delay > this.Lifetime) 
		{
			Destroy (this.gameObject);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("Hit " + collision.gameObject);
		Spaceship victim = collision.gameObject.GetComponent<Spaceship> ();
		if (victim != null) 
		{
			Debug.Log ("Victim not null");
			victim.TakeHit (this.Power);
		}
	}
}
