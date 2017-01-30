using UnityEngine;
using System.Collections;
using System;

public abstract class Spaceship : MonoBehaviour
{
    public int EnginePow;
    public int RotPow;
    public float Cz;
    public float Cx;
    private Rigidbody C_Rigidbody;
    private Transform LocalSpaceShip;
    public Transform CanonLeft;
    public Transform CanonRight;
    public GameObject Projectile;
	public float LaserCoolDown;
	private float LaserDelay;
	public float Armor;
	public float Stamina;
	public float HitPoint;

	// Use this for initialization
	void Start () {
        this.C_Rigidbody = this.GetComponent<Rigidbody>();
        this.LocalSpaceShip = this.transform.FindChild("LocalSpaceShip");
		this.OnStart ();
	}

	virtual protected void OnStart () 
	{
		// Do nothing.
	}
	
	// Update is called once per frame
	void Update () {
		this.OnUpdate ();
		this.LaserDelay -= Time.deltaTime;
	}

	virtual protected void OnUpdate() 
	{
		// Do nothing.
	}
		

    public void FixedUpdate()
    {
        float forwardInput = 0f;
        float rightInput = 0f;

		this.InputControl (out forwardInput, out rightInput);

        C_Rigidbody.AddForce(this.transform.forward * EnginePow * forwardInput);
        C_Rigidbody.AddTorque(this.transform.up * RotPow * rightInput);

        float rightVelocity = (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).x;
        float forwardVelocity = (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).z;

        this.LocalSpaceShip.localRotation = Quaternion.Euler(0, 0, 90f * Mathf.Min(1f, rightVelocity / 25f));

        C_Rigidbody.AddForce(- this.transform.forward * forwardVelocity * Mathf.Abs(forwardVelocity) * this.Cz);
        C_Rigidbody.AddForce(- this.transform.right * rightVelocity * Mathf.Abs(rightVelocity) * this.Cx);
    }

	abstract protected void InputControl (out float forwardInput, out float rightInput);

    public void Shoot()
    {
		if (this.LaserDelay > 0f) {
			return;
		} 
		else 
		{
			this.LaserDelay = this.LaserCoolDown;
		}
		GameObject projectile = GameObject.Instantiate(Projectile);
        projectile.transform.position = this.CanonLeft.position;
        projectile.GetComponent<Rigidbody>().velocity = this.C_Rigidbody.velocity + this.transform.forward * 200f;

		projectile.GetComponent<Projectile> ().Lifetime = 2f;

        projectile = GameObject.Instantiate(Projectile);
        projectile.transform.position = this.CanonRight.position;
        projectile.GetComponent<Rigidbody>().velocity = this.C_Rigidbody.velocity + this.transform.forward * 200f;

		projectile.GetComponent<Projectile> ().Lifetime = 2f;
    }

	public void TakeHit(float amount) 
	{
		float armoredAmout = amount * (1f - this.Armor);
		this.HitPoint -= armoredAmout;
	}
}
