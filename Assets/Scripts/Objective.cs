using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
	private static List<Objective> Instances = new List<Objective>();

	public int Index;

    protected bool activated = false;

    public void Start()
	{
        this.OnStart();
    }

	public void Register() {
		if (!Objective.Instances.Contains (this)) {
			Objective.Instances.Add (this);
		}
	}

    public void Update()
    {
        if (this.activated)
        {
            this.OnUpdate();
        }
    }

    public virtual void OnStart()
    {
        // Do nothing.
    }

    public virtual void OnUpdate()
    {
        // Do nothing.
    }

    public virtual void Activate()
    {
        this.activated = true;
    }

    public virtual void Validate()
    {
        this.activated = false;
		Objective next = this.FindNext ();
        if (next != null)
        {
            next.Activate();
        }
        else
        {
            Debug.Log("You win !");
			GameManager.GameOver (true);
        }
    }

	public static Objective FindFirst() {
		Objective first = null;
		foreach (Objective o in Objective.Instances) {
			if ((first == null) || (first.Index > o.Index)) {
				first = o;

			}
		}
		return first;
	}

	private Objective FindNext() {
		Objective next = null;
		foreach (Objective o in Objective.Instances) {
			if (o.Index > this.Index) {
				if ((next == null) || (next.Index > o.Index)) {
					next = o;
				}
			}
		}
		return next;
	}
}
