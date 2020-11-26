using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty currentProperty;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;

    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropertyPath = string.Empty;

        foreach (SerializedProperty _property in property)
        {
            if (_property.isArray && _property.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                _property.isExpanded = EditorGUILayout.Foldout(_property.isExpanded, _property.displayName);
                EditorGUILayout.EndHorizontal();

                if (_property.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(_property, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropertyPath) && _property.propertyPath.Contains(lastPropertyPath)) { continue; }
                lastPropertyPath = _property.propertyPath;
                EditorGUILayout.PropertyField(_property, drawChildren);
            }
        }
    }

    protected virtual void DrawSideBar(SerializedProperty property)
    {
        foreach (SerializedProperty _property in property)
        {
            if (GUILayout.Button(_property.displayName))
            {
                selectedPropertyPath = _property.propertyPath;
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
        }
    }

    protected void DrawField(string propertyName, bool relative)
    {
        if (relative && serializedObject != null)
        {
            EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propertyName), true);
        }
        else if (serializedObject != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), true);
        }
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}
