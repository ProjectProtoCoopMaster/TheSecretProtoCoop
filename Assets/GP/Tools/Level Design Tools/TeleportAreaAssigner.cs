#if UNITY_STANDALONE
using UnityEngine;
using UnityEditor;
using Gameplay.VR.Player;

public class TeleportAreaAssigner : Editor
{
    [MenuItem("Tools/Assign Teleport Materials %t")]
    static void AssignTeleportationAreas()
    {
        TeleportationArea[] teleportationAreas = FindObjectsOfType<TeleportationArea>();

        for (int i = 0; i < teleportationAreas.Length; i++)
        {
            teleportationAreas[i].GetComponent<Renderer>().material = (Material)AssetDatabase.LoadAssetAtPath("Assets/GP/Features/Player/Feedbacks/Materials/PBR_TeleportArea.mat", typeof(Material));
        }
    }
}
#endif
