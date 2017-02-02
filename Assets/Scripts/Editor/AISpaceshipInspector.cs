using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AISpaceship))]
public class AISpaceshipInspector : Editor
{
    private AISpaceship _target;
    private AISpaceship Target
    {
        get
        {
            if (_target == null)
            {
                _target = (AISpaceship)target;
            }
            return _target;
        }
    }
     
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        EditorGUILayout.LabelField("Dodge Obstacle");
        Target.dodgeForwardInput = EditorGUILayout.FloatField("ForwardInput", Target.dodgeForwardInput);
        Target.dodgeRightInputCoef = EditorGUILayout.FloatField("RightInput Coef", Target.dodgeRightInputCoef);
        Target.dodgeSphereCastRadius = EditorGUILayout.FloatField("SphereCast Radius", Target.dodgeSphereCastRadius);
        Target.dodgeSphereCastDistance = EditorGUILayout.FloatField("SphereCast Distance", Target.dodgeSphereCastDistance);

        EditorGUILayout.LabelField("Escape");
        Target.escapeForwardInput = EditorGUILayout.FloatField("ForwardInput", Target.escapeForwardInput);
        Target.escapeRightInputCoef = EditorGUILayout.FloatField("RightInput Coef", Target.escapeRightInputCoef);

        EditorGUILayout.LabelField("Track");
        Target.trackShortDistance = EditorGUILayout.FloatField("Short Distance", Target.trackShortDistance);
        Target.trackLongDistance = EditorGUILayout.FloatField("Long Distance", Target.trackLongDistance);
        Target.trackMiddleRightInputCoef = EditorGUILayout.FloatField("Medium RightInput Coef", Target.trackMiddleRightInputCoef);
        Target.trackLongForwardInput = EditorGUILayout.FloatField("Long ForwardInput", Target.trackLongForwardInput);
        Target.trackLongRightInputCoef = EditorGUILayout.FloatField("Long RightInput Coef", Target.trackLongRightInputCoef);
    }
}
