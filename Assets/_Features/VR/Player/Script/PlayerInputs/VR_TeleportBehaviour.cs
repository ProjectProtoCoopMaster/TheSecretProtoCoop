#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_TeleportBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Boolean teleportAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource;

        [SerializeField] [FoldoutGroup("Shooting")] TeleportManager teleportationManager = null;

        private void Awake()
        {
            controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;
        }

        private void Update()
        {
            if (teleportAction.GetStateDown(handSource)) 
                teleportationManager.TallRayPointer(controllerPose);

            if (teleportAction.GetStateUp(handSource))
                teleportationManager.TryTeleporting();
        }
    }
}
#endif