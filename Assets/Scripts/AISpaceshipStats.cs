using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpaceshipStats {

	private DateTime TimeStart;
	private int Shots;
	private int AimedShots;
	private int Collisions;

	public AISpaceshipStats() {
		this.TimeStart = DateTime.Now;
	}

	public string GetLogStats() {
		string logStats = "";
		logStats = "Duration : " + (DateTime.Now - TimeStart).TotalSeconds + " seconds. ";
		logStats = "Shots : " + Shots + ". Precision : " + (((float)AimedShots) / Shots * 100f) + " %.";
		return logStats;
	}

	public void AddShot() {
		this.Shots += 1;
	}

	public void AddAimedShot() {
		this.AimedShots += 1;
	}

	public void AddCollision() {
		this.Collisions += 1;
	}
}
