#if UNITY_STANDALONE
using UnityEngine;
using Sirenix.OdinInspector;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_TeleportBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Input_Sources handType;
        [SerializeField] [FoldoutGroup("SteamVR Components")] SteamVR_Behaviour_Pose controllerPose;
        [SerializeField] [FoldoutGroup("SteamVR Components")] SteamVR_Action_Boolean teleportAction;

        private void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    } 
}
#endif