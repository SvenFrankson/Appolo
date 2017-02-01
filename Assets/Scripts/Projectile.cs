using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float Power;
	public float Lifetime;
	public float Velocity;
	private float Delay = 0f;
	private Action<Spaceship> OnHit = Zero.DoNothing;
	private Action OnFail = Zero.DoNothing;

	private Rigidbody c_Rigidbody;
	private Rigidbody C_Rigidbody {
		get { 
			if (c_Rigidbody == null) {
				this.c_Rigidbody = this.GetComponent<Rigidbody> ();
			}
			return this.c_Rigidbody;
		} 
	}

	public void Initialize(Vector3 worldPos, Vector3 direction, Vector3 velocity, Action<Spaceship> onHit = null, Action onFail = null) {
		this.C_Rigidbody.MovePosition (worldPos);
		this.C_Rigidbody.velocity = velocity + direction.normalized * this.Velocity;
		this.OnHit = onHit != null ? onHit : Zero.DoNothing;
		this.OnFail = onFail != null ? onFail : Zero.DoNothing;
	}

	void Update () {
		this.Delay += Time.deltaTime;
		if (this.Delay > this.Lifetime) 
		{
			this.OnFail ();
			Destroy (this.gameObject);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		Spaceship victim = collision.gameObject.GetComponent<Spaceship> ();
		if (victim != null) 
		{
			victim.TakeHit (this.Power);
			this.OnHit (victim);
		}
	}
}
