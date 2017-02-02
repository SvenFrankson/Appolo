using UnityEngine;
using System.Collections;
using System;

public abstract class Spaceship : MonoBehaviour
{
    public enum Team
    {
        TeamA,
        TeamB
    };

    public int EnginePow;
    public int RotPow;
    public float Cz;
    public float Cx;
	protected Rigidbody C_Rigidbody;
    private Transform LocalSpaceShip;
    public Transform CanonLeft;
    public Transform CanonRight;
    public GameObject Projectile;
	public float LaserCoolDown;
	private float LaserDelay;
	public float Armor;
	public float Stamina;
	public float HitPoint;
    public Team SpaceShipTeam;


    protected AISpaceshipStats stats = new AISpaceshipStats();

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

    public bool Shoot()
    {
		if (this.LaserDelay > 0f) {
			return false;
		} 
		else 
		{
			this.LaserDelay = this.LaserCoolDown;
		}
		GameObject projectileInstance = GameObject.Instantiate(Projectile);
		Projectile projectile = projectileInstance.GetComponent<Projectile> ();
		projectile.Initialize (this.CanonLeft.position, this.transform.forward, this.C_Rigidbody.velocity, LogOnHit, LogOnMiss);

		projectileInstance = GameObject.Instantiate(Projectile);
		projectile = projectileInstance.GetComponent<Projectile> ();
		projectile.Initialize (this.CanonRight.position, this.transform.forward, this.C_Rigidbody.velocity, LogOnHit, LogOnMiss);

		return true;
    }

	public void TakeHit(float amount) 
	{
		float armoredAmout = amount * (1f - this.Armor);
		this.HitPoint -= armoredAmout;
        if (this.HitPoint <= 0f)
        {
            Destroy(this.gameObject);
        }
	}

	public void LogStats() {
		Log.Info (this.name, this.stats.GetLogStats ());
	}

	public void LogOnHit(Spaceship victim) {
		this.stats.AddAimedShot ();
		Log.Info (this.name, "Has hit " + victim.name + ".");
	}

	public void LogOnMiss() {
		//Log.Info (this.name, "Missed a shot.");
	}
}
