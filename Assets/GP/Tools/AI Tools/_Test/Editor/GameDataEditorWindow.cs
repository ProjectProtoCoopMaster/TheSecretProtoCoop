using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameDataEditorWindow : ExtendedEditorWindow
{
    bool inventory = false;
    bool character = false;

    public static void Open(GameDataObject dataObject)
    {
        GameDataEditorWindow window = GetWindow<GameDataEditorWindow>("Game Data Editor");
        window.serializedObject = new SerializedObject(dataObject);
    }

    private void OnGUI()
    {
        currentProperty = serializedObject.FindProperty("gameData");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        DrawSideBar(currentProperty);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if (selectedProperty != null) DrawSelectedPropertiesPanel();

        else EditorGUILayout.LabelField("Select an item in the list");

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        Apply();
    }

    void DrawSelectedPropertiesPanel()
    {
        currentProperty = selectedProperty;

        EditorGUILayout.BeginHorizontal();

        DrawField("name", true);
        DrawField("title", true);

        EditorGUILayout.EndHorizontal();

        DrawField("description", true);

        DrawField("image", true);
        
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Inventory", EditorStyles.toolbarButton))
        {
            inventory = true;
            character = false;
        }
        if (GUILayout.Button("Character", EditorStyles.toolbarButton))
        {
            inventory = false;
            character = true;
        }

        EditorGUILayout.EndHorizontal();

        if (character)
        {
            EditorGUILayout.BeginVertical("box");

            DrawField("isCharacterModel", true);
            DrawField("isFriendly", true);
            DrawField("isEnemy", true);
            DrawField("modelData", true);
            DrawField("stats", true);

            EditorGUILayout.EndVertical();
        }
        if (inventory)
        {
            EditorGUILayout.BeginVertical("box");

            DrawField("currentInventorySize", true);
            DrawField("nextInventorySize", true);
            DrawField("maxInventorySize", true);
            DrawField("inventoryUpgradeCost", true);
            DrawField("startingInventory", true);

            EditorGUILayout.EndVertical();
        }
    }
}
