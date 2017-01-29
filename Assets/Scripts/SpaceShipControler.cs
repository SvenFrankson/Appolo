using UnityEngine;
using System.Collections;
using System;

public class SpaceShipControler : MonoBehaviour
{
    private delegate void OutAction<T1, T2>(out T1 forwardInput, out T2 rightInput);

    public int EnginePow;
    public int RotPow;
    public float Cz;
    public float Cx;
    private Rigidbody C_Rigidbody;
    private Transform LocalSpaceShip;
    public Transform CanonLeft;
    public Transform CanonRight;
    public GameObject Projectile;
    public bool IsPlayer;
    public float CloseDistance;
    public float LongDistance;
    public SpaceShipControler Target;
    private OutAction<float, float> InputReceiver;

    public float DistanceMax;
    public float AngleMin;
    public float AngleMax;

	// Use this for initialization
	void Start () {
        this.C_Rigidbody = this.GetComponent<Rigidbody>();
        this.LocalSpaceShip = this.transform.FindChild("LocalSpaceShip");
        if (IsPlayer)
        {
            this.InputReceiver = PlayerInputControl;
        }
        else
        {
            this.FindTarget();
            this.InputReceiver = AIInputControl;
        }
	}

    private void FindTarget()
    {
        SpaceShipControler[] spaceships = FindObjectsOfType<SpaceShipControler>();
        foreach (SpaceShipControler spaceship in spaceships)
        {
            if (spaceship.IsPlayer)
            {
                Target = spaceship;
                return;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Shoot();
        }
	}

    public void FixedUpdate()
    {
        float forwardInput = 0f;
        float rightInput = 0f;

        this.InputReceiver(out forwardInput, out rightInput);

        C_Rigidbody.AddForce(this.transform.forward * EnginePow * forwardInput);
        C_Rigidbody.AddTorque(this.transform.up * RotPow * rightInput);

        float rightVelocity = (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).x;
        float forwardVelocity = (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).z;

        this.LocalSpaceShip.localRotation = Quaternion.Euler(0, 0, 90f * Mathf.Min(1f, rightVelocity / 25f));

        C_Rigidbody.AddForce(- this.transform.forward * forwardVelocity * Mathf.Abs(forwardVelocity) * this.Cz);
        C_Rigidbody.AddForce(- this.transform.right * rightVelocity * Mathf.Abs(rightVelocity) * this.Cx);
    }

    private void PlayerInputControl(out float forwardInput, out float rightInput)
    {
        forwardInput = TouchControler.TouchValue.y;
        rightInput = TouchControler.TouchValue.x;
    }

    private void AIInputControl(out float forwardInput, out float rightInput)
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

        if (Mathf.Abs(angle) < 1f)
        {
            this.Shoot();
        }
    }

    public void OnGUI()
    {
        GUILayout.TextField("Velocity : " + this.C_Rigidbody.velocity);
        GUILayout.TextField("RightVelocity : " + (this.transform.worldToLocalMatrix * this.C_Rigidbody.velocity).x);
    }

    public void Shoot()
    {
        GameObject projectile = GameObject.Instantiate(Projectile);
        projectile.transform.position = this.CanonLeft.position;
        projectile.GetComponent<Rigidbody>().velocity = this.C_Rigidbody.velocity + this.transform.forward * 200f;

        projectile = GameObject.Instantiate(Projectile);
        projectile.transform.position = this.CanonRight.position;
        projectile.GetComponent<Rigidbody>().velocity = this.C_Rigidbody.velocity + this.transform.forward * 200f;
    }
}
