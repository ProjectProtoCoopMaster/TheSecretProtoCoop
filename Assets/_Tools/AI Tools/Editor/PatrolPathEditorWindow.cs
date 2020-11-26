using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using Gameplay.AI;

public class PatrolPathEditorWindow : ExtendedEditorWindow
{
    private string selectedPropertyPath;

    public static void Open(PatrolPath patrolPathObject)
    {
        PatrolPathEditorWindow window = GetWindow<PatrolPathEditorWindow>("Path Patrol Editor");
        window.serializedObject = new SerializedObject(patrolPathObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("waypoints");

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        DrawListBar(currentProperty);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (selectedProperty != null) DrawSelectedWaypointPanel();

        else EditorGUILayout.LabelField("Select a waypoint in the list or create one");

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    protected void DrawSelectedWaypointPanel()
    {
        currentProperty = selectedProperty;

        DrawField("actions", true);
    }

    protected Color ChangeColorTo(string hexaColor)
    {
        Color _color = new Color();
        //ColorUtility.TryParseHtmlString(hexaColor, out _color);
        return _color;
    }

    protected void DrawListBar(SerializedProperty property)
    {
        EditorGUILayout.BeginVertical();

        for (int i = 0; i < property.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Waypoint " + (i + 1)))
            {
                selectedPropertyPath = property.GetArrayElementAtIndex(i).propertyPath;
            }

            Color saveColor = GUI.color;

            GUI.color = new Color32(234, 56, 78, 255);

            if (GUILayout.Button("Delete", GUILayout.MaxWidth(100)))
            {
                DeleteArrayElementOfPropertyAtIndex(property, i);
            }

            GUI.color = saveColor;

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Waypoint +"))
        {
            property.InsertArrayElementAtIndex(property.arraySize);
        }

        EditorGUILayout.EndVertical();

        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        }
    }

    protected void DeleteArrayElementOfPropertyAtIndex(SerializedProperty _property, int index)
    {
        if (_property.GetArrayElementAtIndex(index).objectReferenceValue != null) _property.DeleteArrayElementAtIndex(index);
        _property.DeleteArrayElementAtIndex(index);
    }
}
