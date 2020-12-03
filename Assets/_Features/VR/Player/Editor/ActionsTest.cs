#if UNITY_STANDALONE

using Valve.VR;
using UnityEngine;

public class ActionsTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean teleportAction, shootAction, grabAction;

    bool GetTeleportDown
    {
        get
        {
            return teleportAction.GetStateDown(handType);
        }
    }

    bool GetShootDown
    {
        get
        {
            return shootAction.GetStateDown(handType);
        }
    }

    bool GetGrabDown
    {
        get
        {
            return grabAction.GetStateDown(handType);
        }
    }

    private void Update()
    {
        if (GetTeleportDown) Debug.Log("Telepoted with " + handType);
        if (GetShootDown) Debug.Log("Shot with " + handType);
        if (GetGrabDown) Debug.Log("Grabbed with " + handType);
    }
}


#endif