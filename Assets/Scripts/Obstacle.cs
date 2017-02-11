using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	static private float UpdateTime = 0f;
	static public float S_UpdateTime 
	{
		get 
		{ 
			return Obstacle.UpdateTime;
		}
	}

	private PlayerSpaceship PointOfView;
    private bool Alert;
    private GameObject wrongWay;
    private GameObject WrongWay
    {
        get
        {
            if (this.wrongWay == null)
            {
                this.wrongWay = this.transform.FindChild("WrongWay").gameObject;
            }
            return this.wrongWay;
        }
    }

	public void Start() 
	{
		this.PointOfView = FindObjectOfType<PlayerSpaceship> ();
        this.SetAlert(false);
        this.SwitchAlertFalse();
	}

    public void Update()
    {
        if (this.Alert)
        {
            float rotationSpeed = Vector3.Angle(this.WrongWay.transform.forward, this.PointOfView.transform.forward);
            rotationSpeed = 1f - Mathf.Abs(90f - rotationSpeed) / 90f;
            rotationSpeed = 45f + rotationSpeed * 315f;
            this.WrongWay.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        if (PointOfView != null)
        {
            float t1 = Time.realtimeSinceStartup;
            float alpha = Vector3.Angle(PointOfView.transform.forward, this.transform.position - PointOfView.transform.position);
            if (alpha < PointOfView.Angle)
            {
                float sqrDist = (this.transform.position - PointOfView.transform.position).sqrMagnitude;
                if (sqrDist < PointOfView.DistanceMax * PointOfView.DistanceMax)
                {
                    SetAlert(true);
                    UpdateTime += Time.realtimeSinceStartup - t1;
                    return;
                }
            }
            SetAlert(false);
            UpdateTime += Time.realtimeSinceStartup - t1;
        }
    }

    public void SetAlert(bool alert)
    {
        if (Alert != alert)
        {
            Alert = alert;
            if (Alert)
            {
                SwitchAlertTrue();
            }
            else
            {
                SwitchAlertFalse();
            }
        }
    }

    public void SwitchAlertTrue()
    {
        this.WrongWay.SetActive(true);
        this.WrongWay.transform.LookAt(PointOfView.transform);
    }

    public void SwitchAlertFalse()
    {
        this.WrongWay.SetActive(false);
    }
}
