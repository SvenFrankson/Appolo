using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

static public class Log {

//	public enum Level {
//		INFO,
//		WARNING,
//		ERROR,
//		FATAL,
//		HOLYSHIT
//	}

	static private StreamWriter output;
	static private int LogLevel = 5;

	static Log() {
		output = new StreamWriter ("output.log", false);
		string line = "Time : " + DateTime.Now;
		output.WriteLine (line);
		output.Close ();
	}

	static public void Info(object o) {
		output = new StreamWriter ("output.log", true);
		string line = Time.realtimeSinceStartup + " INFO " + o;
		output.WriteLine (line);
		output.Close ();
	}
}
