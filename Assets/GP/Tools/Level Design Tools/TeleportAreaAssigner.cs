using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gameplay.VR.Player;

public class TeleportAreaAssigner : Editor
{
    TeleportationArea[] teleportationAreas;

    [MenuItem("Tools/Assign Teleport Materials %t")]
    private void AssignTeleportationAreas()
    {
        teleportationAreas = FindObjectsOfType<TeleportationArea>();

        for (int i = 0; i < teleportationAreas.Length; i++)
        {
            teleportationAreas[i].GetComponent<Renderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/GP/Features/Player/Feedbacks/Materials/PBR_TeleportArea.mat", typeof(Material));
        }        
    }
}
