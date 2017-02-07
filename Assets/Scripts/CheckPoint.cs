using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : Objective
{
    public float Size;
    private float sqrSize;
    private PlayerSpaceship playerSpaceship;
	private GameObject deactivatedModel;
	private GameObject DeactivatedModel {
		get { 
			if (this.deactivatedModel == null) {
				this.deactivatedModel = this.transform.Find ("DeactivatedModel").gameObject;
			}
			return this.deactivatedModel;
		}
	}
	private GameObject activatedModel;
	private GameObject ActivatedModel {
		get { 
			if (this.activatedModel == null) {
				this.activatedModel = this.transform.Find ("ActivatedModel").gameObject;
			}
			return this.activatedModel;
		}
	}

    public override void OnStart()
    {
        this.sqrSize = this.Size * this.Size;
        this.playerSpaceship = FindObjectOfType<PlayerSpaceship>();
    }

    public override void OnUpdate()
    {
        float sqrDist = (this.playerSpaceship.transform.position - this.transform.position).sqrMagnitude;
        if (sqrDist < sqrSize)
        {
            this.Validate();
        }
    }

    public override void Activate()
	{
        base.Activate();
		this.DeactivatedModel.SetActive(false);
		this.ActivatedModel.SetActive(true);
    }

    public override void Validate()
    {
        base.Validate();
		this.DeactivatedModel.SetActive(true);
		this.ActivatedModel.SetActive(false);
    }
}
