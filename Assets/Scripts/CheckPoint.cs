using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : Objective
{
    public float Size;
    public float sqrSize;
    public PlayerSpaceship playerSpaceship;
    public GameObject deactivatedModel;
    public GameObject activatedModel;

    public override void OnStart()
    {
        this.sqrSize = this.Size * this.Size;
        this.playerSpaceship = FindObjectOfType<PlayerSpaceship>();
        Transform[] children = this.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in children)
        {
            if (t.name == "DeactivatedModel")
            {
                this.deactivatedModel = t.gameObject;
            }
            else if (t.name == "ActivatedModel")
            {
                this.activatedModel = t.gameObject;
            }
        }
        Debug.Log(".");
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
        this.deactivatedModel.SetActive(false);
        this.activatedModel.SetActive(true);
    }

    public override void Validate()
    {
        base.Validate();
        this.deactivatedModel.SetActive(true);
        this.activatedModel.SetActive(false);
    }
}
