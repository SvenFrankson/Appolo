using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {

    public Transform CamTarget;
    public Transform SpaceShip;
    public float Smoothness;

	void FixedUpdate () {
        this.transform.position = this.transform.position * this.Smoothness + CamTarget.transform.position * (1f - this.Smoothness);
        this.transform.LookAt(SpaceShip, Vector3.up);
	}
}
