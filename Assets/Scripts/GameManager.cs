using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	static private GameManager instance;
	static private GameManager Instance {
		get { 
			if (GameManager.instance == null) {
				GameManager.instance = FindObjectOfType<GameManager> ();
			}
			return GameManager.instance;
		}
	}

	static private bool gameOn = false;
	static public bool GameOn {
		get { 
			return GameManager.gameOn;
		}
	}

	public Spaceship PlayerSpaceshipPrefab;
	private Spaceship PlayerSpaceshipInstance;

	public List<Level> Levels;
	private Level LevelInstance;

	public void Start() {
		this.PlayerSpaceshipInstance = Instantiate<Spaceship> (this.PlayerSpaceshipPrefab);
	}

	static public void GameOver(bool hasWon) {
		Debug.Log ("Game Over : " + hasWon);
		Instance.CleanUp ();
		GameManager.gameOn = false;
	}

	public void LoadLevel(Level levelPrefab) {
		this.CleanUp ();
		this.LevelInstance = Instantiate<Level> (levelPrefab);
		this.LevelInstance.name = levelPrefab.name;
		GameManager.gameOn = true;
	}

	public void CleanUp() {
		// PlayerSpaceshipPrefab
		if (this.PlayerSpaceshipInstance != null) {
			PlayerSpaceshipInstance.Reset();
		}
		// Camera
		// Do nothing

		// Level
		if (this.LevelInstance != null) {
			this.LevelInstance.Unload ();
		}
	}

	public void OnGUI() {
		if (!GameManager.gameOn) {
			foreach (Level level in this.Levels) {
				if (GUILayout.Button (level.name)) {
					this.LoadLevel (level);
				}
			}
		}
	}
}
