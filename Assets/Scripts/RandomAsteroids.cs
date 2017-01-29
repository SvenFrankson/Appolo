using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAsteroids : MonoBehaviour {

    public int Radius;
    public int HeightRange;
    public int Count;
    public GameObject[] Asteroids;
    public Material[] Materials;

    public void Generate()
    {
        GameObject container = new GameObject();
        container.name = "Asteroids";
        for (int i = 0; i < Count; i++)
        {
            float x = UnityEngine.Random.Range(-Radius, Radius);
            float y = UnityEngine.Random.Range(0f, HeightRange);
            float z = UnityEngine.Random.Range(-Radius, Radius);
            float x0 = UnityEngine.Random.Range(0, 180f);
            float y0 = UnityEngine.Random.Range(0, 180f);
            float z0 = UnityEngine.Random.Range(0, 180f);

            int Ast = UnityEngine.Random.Range(0, Asteroids.Length - 1);
            int Mat = UnityEngine.Random.Range(0, Materials.Length - 1);

            GameObject g = GameObject.Instantiate<GameObject>(Asteroids[Ast]);
            g.GetComponent<Renderer>().sharedMaterial = Materials[Mat];
            g.transform.position = new Vector3(x, y, z);
            g.transform.rotation = Quaternion.Euler(x0, y0, z0);
            g.transform.parent = container.transform;
        }
    }
}
