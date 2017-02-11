using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositionTarget : MonoBehaviour {

    public float MenuRotationSpeed;
    private Vector3 positionZero;

	void Start ()
    {
        this.positionZero = this.transform.localPosition;
	}
	
	void FixedUpdate ()
    {
		if (GameManager.GameOn)
        {
            this.transform.localPosition = this.positionZero;
        }
        else
        {
            this.transform.RotateAround(this.transform.parent.position, Vector3.up, this.MenuRotationSpeed * Time.fixedDeltaTime);
        }
	}
}
