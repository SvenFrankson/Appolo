using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnerInspector : Editor
{
    private Spawner _target;
    private Spawner Target
    {
        get
        {
            if (_target == null)
            {
                _target = (Spawner)target;
            }
            return _target;
        }
    }

    public override void OnInspectorGUI()
    {
        Target.ActivateOnStart = EditorGUILayout.Toggle("Activate On Start", Target.ActivateOnStart);
        if (Target.Spawns != null)
        {
            for (int i = 0; i < Target.Spawns.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Spawn " + i + " / Delay");
                Target.Spawns[i] = EditorGUILayout.ObjectField(Target.Spawns[i], typeof(GameObject), false) as GameObject;
                Target.Delays[i] = EditorGUILayout.FloatField(Target.Delays[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-"))
        {
            Target.Reduce();
        }
        if (GUILayout.Button("+"))
        {
            Target.Extend();
        }
        EditorGUILayout.EndHorizontal();
    }
}
