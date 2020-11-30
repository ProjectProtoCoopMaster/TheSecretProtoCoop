#if UNITY_STANDALONE
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_TeleportBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Boolean teleportAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource;

        [SerializeField] AgentVRTeleportationManager teleportationManager = null;

        private void Awake()
        {
            controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;
        }

        void OnEnable()
        {
            teleportAction.AddOnStateDownListener(ShowPointer, handSource);
            teleportAction.AddOnStateUpListener(TryTeleport, handSource);
        }

        private void ShowPointer(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            teleportationManager.TallRayPointer(controllerPose);
        }

        private void TryTeleport(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            teleportationManager.TryTeleporting();
        }
    }
}
#endif