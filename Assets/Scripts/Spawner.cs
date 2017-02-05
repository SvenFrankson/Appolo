using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool ActivateOnStart;
    private bool Activated;
    private float TimeSinceActivation;
    public List<GameObject> Spawns;
    public List<float> Delays;
    public Spawner Next;

    public void Extend()
    {
        if (this.Spawns == null)
        {
            this.Spawns = new List<GameObject>();
        }
        if (this.Delays == null)
        {
            this.Delays = new List<float>();
        }

        this.Spawns.Add(null);
        this.Delays.Add(0f);
    }

    public void Reduce()
    {
        if (this.Spawns == null ||
            this.Delays == null ||
            this.Spawns.Count == 0 ||
            this.Delays.Count == 0)
        {
            return;
        }

        this.Spawns.RemoveAt(this.Spawns.Count - 1);
        this.Delays.RemoveAt(this.Delays.Count - 1);
    }

    public void Start()
    {
        if (this.ActivateOnStart)
        {
            this.Activate();
        }
    }

    public void Activate()
    {
        this.Activated = true;
    }

    public void Update()
    {
        if (!this.Activated)
        {
            return;
        }

        this.TimeSinceActivation += Time.deltaTime;

        for (int i = 0; i < this.Spawns.Count; i++)
        {
            if (this.Delays[i] < this.TimeSinceActivation)
            {
                GameObject spawn = GameObject.Instantiate(this.Spawns[i], this.transform.position, this.transform.rotation);
                spawn.name = "Spawn_" + i;
                this.Spawns.RemoveAt(i);
                this.Delays.RemoveAt(i);
                return;
            }
        }

        if (this.Spawns.Count == 0)
        {
            this.Deactivate();
        }
    }

    public void Deactivate()
    {
        this.Activated = false;
        if (this.Next != null)
        {
            this.Next.Activate();
        }
        else
        {
            // Game over, you win.
            Debug.Log("You win !");
        }
    }
}
