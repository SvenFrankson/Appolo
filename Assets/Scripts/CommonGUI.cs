using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonGUI : MonoBehaviour {

	// Game informations. To be transfered to pretty GUI.
	public PlayerSpaceship PlayerSpaceship;
	public AISpaceship[] AISpaceships;

	public void Start() {
		this.PlayerSpaceship = FindObjectOfType<PlayerSpaceship> ();
		this.AISpaceships = FindObjectsOfType<AISpaceship> ();
	}

	public void OnGUI()
	{
		// Performance metrics
		GUILayout.TextArea ("Obstacle computation time : " + (Obstacle.S_UpdateTime / Time.realtimeSinceStartup * 100f) + " %");
		GUILayout.TextArea ("AI control time : " + (AISpaceship.S_ControlTime / Time.realtimeSinceStartup * 100f) + " %");
	
		// Game informations. To be transfered to pretty GUI.
		GUILayout.TextArea("Player : " + PlayerSpaceship.HitPoint + " / " + PlayerSpaceship.Stamina);
		for (int i = 0; i < AISpaceships.Length; i++) {
			GUILayout.TextArea ("Foe " + i + " : " + AISpaceships [i].HitPoint + " / " + AISpaceships [i].Stamina);
		}
	}
}
