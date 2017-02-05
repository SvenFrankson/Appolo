using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RuntimeTest : MonoBehaviour
{
    public bool ShowConsole = false;
    public Spaceship TargetSpaceship;
    public Rigidbody TargetRigidbody;

    public void Start()
    {
        this.TargetSpaceship = this.GetComponent<Spaceship>();
        this.TargetRigidbody = this.GetComponent<Rigidbody>();
    }

    public void OnGUI()
    {
        if (!this.ShowConsole)
        {
            if (GUILayout.Button("Show"))
            {
                this.ShowConsole = true;
            }
        }
        else if (this.ShowConsole)
        {
            if (GUILayout.Button("Hide"))
            {
                this.ShowConsole = false;
            }
            // Mass
            GUILayout.BeginHorizontal();
            GUILayout.Label("Mass " + this.TargetRigidbody.mass.ToString("0.0"));
            if (GUILayout.Button("-"))
            {
                this.TargetRigidbody.mass = this.TargetRigidbody.mass - 0.1f;
            }
            if (GUILayout.Button("+"))
            {
                this.TargetRigidbody.mass = this.TargetRigidbody.mass + 0.1f;
            }
            GUILayout.EndHorizontal();
            // Angular Drag
            GUILayout.BeginHorizontal();
            GUILayout.Label("Angular Drag " + this.TargetRigidbody.angularDrag.ToString("0.0"));
            if (GUILayout.Button("-"))
            {
                this.TargetRigidbody.angularDrag = this.TargetRigidbody.angularDrag - 0.1f;
            }
            if (GUILayout.Button("+"))
            {
                this.TargetRigidbody.angularDrag = this.TargetRigidbody.angularDrag + 0.1f;
            }
            GUILayout.EndHorizontal();
            // Engine Pow
            GUILayout.BeginHorizontal();
            GUILayout.Label("Engine Pow " + this.TargetSpaceship.EnginePow);
            if (GUILayout.Button("-"))
            {
                this.TargetSpaceship.EnginePow = this.TargetSpaceship.EnginePow - 5;
            }
            if (GUILayout.Button("+"))
            {
                this.TargetSpaceship.EnginePow = this.TargetSpaceship.EnginePow + 5;
            }
            GUILayout.EndHorizontal();
            // Rot Pow
            GUILayout.BeginHorizontal();
            GUILayout.Label("Rot Pow " + this.TargetSpaceship.RotPow);
            if (GUILayout.Button("-"))
            {
                this.TargetSpaceship.RotPow = this.TargetSpaceship.RotPow - 1;
            }
            if (GUILayout.Button("+"))
            {
                this.TargetSpaceship.RotPow = this.TargetSpaceship.RotPow + 1;
            }
            GUILayout.EndHorizontal();
            // Cz
            GUILayout.BeginHorizontal();
            GUILayout.Label("Cz " + this.TargetSpaceship.Cz.ToString("0.00"));
            if (GUILayout.Button("-"))
            {
                this.TargetSpaceship.Cz = this.TargetSpaceship.Cz - 0.01f;
            }
            if (GUILayout.Button("+"))
            {
                this.TargetSpaceship.Cz = this.TargetSpaceship.Cz + 0.01f;
            }
            GUILayout.EndHorizontal();
            // Cx
            GUILayout.BeginHorizontal();
            GUILayout.Label("Cx " + this.TargetSpaceship.Cx.ToString("0.00"));
            if (GUILayout.Button("-"))
            {
                this.TargetSpaceship.Cx = this.TargetSpaceship.Cx - 0.05f;
            }
            if (GUILayout.Button("+"))
            {
                this.TargetSpaceship.Cx = this.TargetSpaceship.Cx + 0.05f;
            }
            GUILayout.EndHorizontal();
        }
    }
}
