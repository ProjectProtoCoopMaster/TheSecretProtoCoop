using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gameplay.AI;

//[CustomEditor(typeof(PatrolPath))]
public class PatrolPathCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            PatrolPathEditorWindow.Open((PatrolPath)target);
        }
    }
}
