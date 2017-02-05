using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship3DInfo : MonoBehaviour
{
    private Spaceship spaceshipTarget;
    private AISpaceship aiSpaceshipTarget;
    private TextMesh textMesh;

	void Start () {
        spaceshipTarget = this.transform.parent.gameObject.GetComponent<Spaceship>();
        aiSpaceshipTarget = this.transform.parent.gameObject.GetComponent<AISpaceship>();
        textMesh = this.GetComponentInChildren<TextMesh>();
    }
	
	void Update () {
        this.transform.LookAt(Camera.main.transform);
        UpdateText();
	}

    void UpdateText()
    {
        if (aiSpaceshipTarget != null)
        {
            textMesh.text = "" + aiSpaceshipTarget.CurrentAIMode + "\n";
            textMesh.text += "FInput = " + aiSpaceshipTarget.ForwardInput + "\n";
            textMesh.text += "RInput = " + aiSpaceshipTarget.RightInput + "\n";
        }
    }
}
