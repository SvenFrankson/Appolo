using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Objective
{
    private float TimeSinceActivation = 0f;
    public List<GameObject> Spawns;
    private List<GameObject> InvokedSpawns = new List<GameObject>();
    public List<float> Delays;

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

        if (this.Spawns.Count == 0)
        {
            this.Spawns.Add(null);
            this.Delays.Add(0f);
        }
        else
        {
            this.Spawns.Add(this.Spawns[this.Spawns.Count - 1]);
            this.Delays.Add(this.Delays[this.Delays.Count - 1]);
        }
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
   
    public override void OnUpdate()
    {
        this.TimeSinceActivation += Time.deltaTime;

        for (int i = 0; i < this.Spawns.Count; i++)
        {
            if (this.Delays[i] < this.TimeSinceActivation)
            {
                GameObject spawn = GameObject.Instantiate(this.Spawns[i], this.transform.position, this.transform.rotation);
                spawn.name = "Spawn_" + i;
                this.InvokedSpawns.Add(spawn);

                this.Spawns.RemoveAt(i);
                this.Delays.RemoveAt(i);
                return;
            }
        }

        // If all spawns have been invoked.
        if (this.Spawns.Count == 0)
        {
            // Return if any spawn is still alive
            foreach (GameObject g in this.InvokedSpawns)
            {
                if (g != null)
                {
                    return;
                }
            }
            // Validate if all spawns are dead.
            this.Validate();
        }
    }
}
