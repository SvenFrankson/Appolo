using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour {
    
    private Transform camPositionTarget;
	private Transform CamPositionTarget {
		get { 
			if (this.camPositionTarget == null) {
				this.camPositionTarget = GameObject.Find ("CamPositionTarget").transform;
			}
			return this.camPositionTarget;
		}
	}

    private Transform camSpaceshipTarget;
	private Transform CamSpaceshipTarget {
		get {
			if (this.camSpaceshipTarget == null) {
				this.camSpaceshipTarget = GameObject.Find ("CamSpaceshipTarget").transform;
			}
			return this.camSpaceshipTarget;
		}
	}

    public float Smoothness;

	public void FixedUpdate () {
		if (this.CamPositionTarget != null && this.CamSpaceshipTarget != null) {
            this.transform.position = this.transform.position * this.Smoothness + CamPositionTarget.transform.position * (1f - this.Smoothness);
            this.transform.LookAt(CamSpaceshipTarget, Vector3.up);
		}
	}
}
