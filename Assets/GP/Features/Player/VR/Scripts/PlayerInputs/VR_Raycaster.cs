#if UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_Raycaster : LaserPointer
    {
        [SerializeField] protected SteamVR_Action_Boolean clickAction = null;
        protected SteamVR_Behaviour_Pose controllerPose = null;
        protected SteamVR_Input_Sources handSource;

        protected MaterialPropertyBlock clickedColor;

        [SerializeField] protected Color clickColor;

        [SerializeField] protected GameEvent onHover, onClick;

        new void Awake()
        {
            base.Awake();

            controllerPose = GetComponentInParent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;
        }
    }
}
#endif
