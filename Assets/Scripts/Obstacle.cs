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

	public PlayerSpaceship PointOfView;
    private bool Alert = false;

	public void Start() 
	{
		this.PointOfView = FindObjectOfType<PlayerSpaceship> ();
	}

    public void Update()
    {
        if (PointOfView != null)
        {
            float t1 = Time.realtimeSinceStartup;
            float alpha = Vector3.Angle(PointOfView.transform.forward, this.transform.position - PointOfView.transform.position);
            if (alpha < PointOfView.AngleMin)
            {
                SetAlert(true);
                UpdateTime += Time.realtimeSinceStartup - t1;
                return;
            }
            if (alpha < PointOfView.AngleMax)
            {
                float sqrDist = (this.transform.position - PointOfView.transform.position).sqrMagnitude;
                float dist = (alpha - PointOfView.AngleMin) / (PointOfView.AngleMax - PointOfView.AngleMin) * PointOfView.DistanceMax;
                if (sqrDist < dist * dist)
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
