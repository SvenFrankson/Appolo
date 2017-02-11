using UnityEngine;
using System.Collections;
using System;

public class Spaceship : MonoBehaviour
{
    public enum Team
    {
        TeamA,
        TeamB
    };

	protected SpaceshipControler controler;
	protected SpaceshipControler Controller {
		get { 
			if (this.controler == null) {
				this.controler = this.GetComponent<SpaceshipControler> ();
			}
			return this.controler;
		}
	}

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
	private float laserDelay;
	public float Armor;
	public float Stamina;
	private float hitPoint;
	public float HitPoint {
		get { 
			return this.hitPoint;
		}
	}
    public Team SpaceShipTeam;

    private float forwardInput;
    public float ForwardInput
    {
        get
        {
            return this.forwardInput;
        }
    }
    private float rightInput;
    public float RightInput
    {
        get
        {
            return this.rightInput;
        }
    }



    public AISpaceshipStats stats = new AISpaceshipStats();

	// Use this for initialization
	void Start () {
        this.C_Rigidbody = this.GetComponent<Rigidbody>();
        this.LocalSpaceShip = this.transform.FindChild("LocalSpaceShip");
		this.Reset ();
	}

	public void Reset() {
		this.hitPoint = this.Stamina;
		this.laserDelay = 0f;
		this.C_Rigidbody.velocity = Vector3.zero;
		this.C_Rigidbody.MovePosition (Vector3.zero);
		this.C_Rigidbody.MoveRotation (Quaternion.identity);
		this.forwardInput = 0f;
		this.rightInput = 0f;
		this.ClearTrailsAsync ();
	}
	
	// Update is called once per frame
	void Update () {
		this.laserDelay -= Time.deltaTime;
	}

    public void FixedUpdate()
    {
		Controller.InputControl (out this.forwardInput, out this.rightInput);

        this.forwardInput = Mathf.Min(this.forwardInput, 1f);
        this.forwardInput = Mathf.Max(this.forwardInput, -1f);
        this.rightInput = Mathf.Min(this.rightInput, 1f);
        this.rightInput = Mathf.Max(this.rightInput, -1f);

        C_Rigidbody.AddForce(this.transform.forward * EnginePow * this.forwardInput);
        C_Rigidbody.AddTorque(this.transform.up * RotPow * this.rightInput);

        float rightVelocity = (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).x;
        float forwardVelocity = (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).z;

        this.LocalSpaceShip.localRotation = Quaternion.Euler(0, 0, 90f * Mathf.Min(1f, rightVelocity / 25f));

        C_Rigidbody.AddForce(- this.transform.forward * forwardVelocity * Mathf.Abs(forwardVelocity) * this.Cz);
        C_Rigidbody.AddForce(- this.transform.right * rightVelocity * Mathf.Abs(rightVelocity) * this.Cx);
    }

    public bool Shoot()
    {
		if (this.laserDelay > 0f) {
			return false;
		} 
		else 
		{
			this.laserDelay = this.LaserCoolDown;
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
		this.hitPoint -= armoredAmout;
        if (this.hitPoint <= 0f)
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

	#region Graphic
	public void ClearTrailsAsync() {
		StartCoroutine (ClearTrails());
	}

	public IEnumerator ClearTrails() {
		yield return false;
		TrailRenderer[] trails = this.GetComponentsInChildren<TrailRenderer> ();
		foreach (TrailRenderer trail in trails) {
			trail.Clear ();
		}
	}
	#endregion
}
