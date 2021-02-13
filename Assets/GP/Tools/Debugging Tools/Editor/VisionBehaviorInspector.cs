#if UNITY_STANDALONE
using Gameplay.VR;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Tools.Debugging
{
    [ExecuteAlways]
    [CanEditMultipleObjects]
    [CustomEditor(typeof(VisionBehavior))]
    public class VisionBehaviorInspector : Editor
    {
        bool existingData, localData;
        SerializedProperty entityVisionScriptableProp, rangeOfVisionProp, coneOfVisionProp, playerTransformProp, playerDetectionLayerProp;

        VisionBehavior entityVisionDataInterface;
        DetectionBehavior detectionBehavior;
        OverwatchBehavior overwatchBehavior;

        private void OnEnable()
        {
            entityVisionDataInterface = target as VisionBehavior;
            detectionBehavior = entityVisionDataInterface.gameObject.GetComponent<DetectionBehavior>();
            overwatchBehavior = entityVisionDataInterface.gameObject.GetComponent<OverwatchBehavior>();

            rangeOfVisionProp = serializedObject.FindProperty(nameof(entityVisionDataInterface.rangeOfVision));
            coneOfVisionProp = serializedObject.FindProperty(nameof(entityVisionDataInterface.coneOfVision));
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(playerTransformProp);

            EditorGUILayout.Space();

            if (entityVisionScriptableProp.objectReferenceValue != null || existingData) DrawScriptableObjProperty();
            else if (rangeOfVisionProp.floatValue != 0 || coneOfVisionProp.floatValue != 0 || localData) DrawLocalProperties();
            else UserChoice();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            /*Handles.color = Color.red;
            foreach (GameObject guard in overwatchBehavior.awarenessManager.deadGuards)
                Handles.DrawLine(overwatchBehavior.gameObject.transform.position, guard.transform.position);
            Handles.color = Color.white;*/
        }

        #region User Input
        // ask the user if he's assigning or creating data
        private void UserChoice()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("Apply an existing data", "Open an objectfield that allows you to assign pre-existing data. Useful for re-using data that is the same for other entities.")))
            {
                existingData = true;
                localData = false;

            }

            if (GUILayout.Button(new GUIContent("Create local data", "Opens property fields for you to set the entity's data. Useful for creating data that won't be the same for other entities")))
            {
                localData = true;
                existingData = false;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
#endregion

#region Draw Fields
        // display a single property field for assigning exsiting scriptableObject data
        private void DrawScriptableObjProperty()
        {
            EditorGUILayout.BeginVertical();

            if (entityVisionScriptableProp.objectReferenceValue != null)
            {
                SerializedObject entityVisionScriptableObject = new SerializedObject(entityVisionScriptableProp.objectReferenceValue);
                detectionBehavior.rangeOfVision = overwatchBehavior.rangeOfVision = entityVisionScriptableObject.FindProperty("rangeOfVision").floatValue;
                detectionBehavior.coneOfVision = overwatchBehavior.coneOfVision = entityVisionScriptableObject.FindProperty("coneOfVision").floatValue;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(entityVisionScriptableProp);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button(new GUIContent("Change Data", "Go back to select data")))
            {
                entityVisionScriptableProp.objectReferenceValue = null;
                existingData = false;
            }

            EditorGUILayout.EndVertical();


        }

        // draw a property field for creating custom data
        private void DrawLocalProperties()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.EndHorizontal();

            detectionBehavior.rangeOfVision = overwatchBehavior.rangeOfVision = rangeOfVisionProp.floatValue = EditorGUILayout.FloatField(new GUIContent("Range of Vision", "Set the entity's range of vision (expressed in Unity's base units)."), rangeOfVisionProp.floatValue);
            detectionBehavior.coneOfVision = overwatchBehavior.coneOfVision = coneOfVisionProp.floatValue = EditorGUILayout.FloatField(new GUIContent("Cone of Vision", "Set the entity's cone of vision (expressed in degrees)."), coneOfVisionProp.floatValue);

            if (GUILayout.Button(new GUIContent("Change Data", "Go back to select data")))
            {
                rangeOfVisionProp.floatValue = 0;
                coneOfVisionProp.floatValue = 0;
                localData = false;
            }
            EditorGUILayout.EndVertical();
        }
#endregion
    }
} 
#endif