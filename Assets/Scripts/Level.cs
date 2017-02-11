using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public List<GameObject> Props;
	public List<Objective> Objectives;

	public void Start() {
		this.Load ();
		Objective.FindFirst ().Activate ();
	}

	public void Load() {
		foreach (GameObject p in this.Props) {
			GameObject instance = Instantiate (p);
			instance.transform.parent = this.transform;
		}
		foreach (Objective o in Objectives) {
			Objective instance = Instantiate (o);
			instance.transform.parent = this.transform;
			instance.Register ();
		}
	}

	public void Unload() {
		Destroy (this.gameObject);
	}
}
