using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public SpaceShipControler SpaceShip;
    private bool Alert = false;

    public void Update()
    {
        float alpha = Vector3.Angle(SpaceShip.transform.forward, this.transform.position - SpaceShip.transform.position);
        if (alpha < SpaceShip.AngleMin)
        {
            SetAlert(true);
            return;
        }
        if (alpha < SpaceShip.AngleMax)
        {
            float sqrDist = (this.transform.position - SpaceShip.transform.position).sqrMagnitude;
            float dist = (alpha - SpaceShip.AngleMin) / (SpaceShip.AngleMax - SpaceShip.AngleMin) * SpaceShip.DistanceMax;
            if (sqrDist < dist * dist)
            {
                SetAlert(true);
                return;
            }
        }
        SetAlert(false);
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
        HighLight(Color.red, 0.005f);
    }

    public void SwitchAlertFalse()
    {
        HighLight(Color.black, 0.005f);
    }

    public void HighLight(Color color, float outlineWidth)
    {
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();

        foreach (Renderer r in renderers)
        {
            materials.AddRange(r.GetComponent<Renderer>().materials);
        }

        foreach (Material material in materials)
        {
            material.SetColor("_OutlineColor", color);
            material.SetFloat("_Outline", outlineWidth);
        }
    }
}
