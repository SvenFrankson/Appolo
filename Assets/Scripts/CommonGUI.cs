using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonGUI : MonoBehaviour {

	public void OnGUI()
	{
		GUILayout.TextArea ("Obstacle computation time : " + (Obstacle.S_UpdateTime / Time.realtimeSinceStartup * 100f) + " %");
	}
}
