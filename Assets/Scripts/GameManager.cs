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

    public GUISkin Skin;

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
        GUI.skin = this.Skin;
        GUILabelResponsive(20, 50, 20, 50, "Appolo");
		if (!GameManager.gameOn) {
            for (int i = 0; i < this.Levels.Count; i++)
            {
                int x = i % 4;
                int y = i / 4;
                if (GUIButtonResponsive(y * 20 + 60, (x + 1) * 20, 15, 15, this.Levels[i].name))
                {
                    this.LoadLevel(this.Levels[i]);
                }
            }
		}
	}

    public void GUILabelResponsive(int top, int right, int height, int width, string text)
    {
        GUILabelResponsive(top / 100f, right / 100f, height / 100f, width / 100f, text);
    }

    public void GUILabelResponsive(float top, float right, float height, float width, string text)
    {
        int h = Screen.height;
        int w = Screen.width;

        int pHeight = Mathf.RoundToInt(height * h);
        int pWidth = Mathf.RoundToInt(width * w);
        int pTop = Mathf.RoundToInt(top * h) - pHeight / 2;
        int pRight = Mathf.RoundToInt(right * w) - pWidth / 2;

        Rect position = new Rect(pRight, pTop, pWidth, pHeight);

        GUI.Label(position, text, "MainTitle");
    }

    public bool GUIButtonResponsive(int top, int right, int height, int width, string text)
    {
        return GUIButtonResponsive(top / 100f, right / 100f, height / 100f, width / 100f, text);
    }

    public bool GUIButtonResponsive(float top, float right, float height, float width, string text)
    {
        int h = Screen.height;
        int w = Screen.width;

        int pHeight = Mathf.RoundToInt(height * h);
        int pWidth = Mathf.RoundToInt(width * w);
        int pTop = Mathf.RoundToInt(top * h) - pHeight / 2;
        int pRight = Mathf.RoundToInt(right * w) - pWidth / 2;

        Rect position = new Rect(pRight, pTop, pWidth, pHeight);

        return GUI.Button(position, text, "MainLevel");
    }
}
