using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public List<GameObject> Props;
	public List<Objective> Objectives;

	public void Start() {
		this.Load ();
	}

	public void Load() {
		foreach (GameObject p in this.Props) {
			Instantiate (p);
		}
		foreach (Objective o in Objectives) {
			Instantiate (o);
		}
	}
}
