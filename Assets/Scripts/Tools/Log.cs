using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour {

	public enum Level {
		INFO,
		WARNING,
		ERROR,
		FATAL,
		HOLYSHIT
	}
    
    static private Dictionary<string, StreamWriter> LogFiles;
	static public event Action BeforeApplicationQuit;

	static Log() {
        Log.LogFiles = new Dictionary<string, StreamWriter>();
	}

    public void OnApplicationQuit()
    {
		BeforeApplicationQuit ();
        foreach (StreamWriter logFile in LogFiles.Values)
        {
            string line = "Time : " + DateTime.Now + " logfile now closed.";
            logFile.WriteLine(line);
            logFile.Close();
        }
    }

    static private StreamWriter GetStreamWriter(string fileName)
    {
        if (!LogFiles.ContainsKey(fileName))
        {
            StreamWriter logFile = new StreamWriter(fileName + ".log");
            string line = "Time : " + DateTime.Now;
            logFile.WriteLine(line);
            LogFiles.Add(fileName, logFile);
        }

        return LogFiles[fileName];
    }

	static public void Info(string fileName, object o) {
        StreamWriter logFile = GetStreamWriter(fileName);
		string line = Time.realtimeSinceStartup + " INFO " + o;
        logFile.WriteLine (line);
	}
}
