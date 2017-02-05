using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    public bool ActivateOnStart;
    public Objective Next;

    protected bool activated;

    public void Start()
    {
        this.OnStart();
        if (this.ActivateOnStart)
        {
            this.Activate();
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
        if (this.Next != null)
        {
            this.Next.Activate();
        }
        else
        {
            Debug.Log("You win !");
        }
    }
}
