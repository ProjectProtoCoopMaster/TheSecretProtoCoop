#if UNITY_STANDALONE
using Gameplay;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gameplay.VR.Feedbacks
{
    public class UI_InitCameraLayering : MonoBehaviour
    {
        [SerializeField] StringVariable playerCamera;
        Camera uiCamera;
        UniversalAdditionalCameraData targetCamera;

        public void GE_RefreshScene()
        {
            if (uiCamera == null) uiCamera = GetComponentInChildren<Camera>();
            if (targetCamera == null) targetCamera = GameObject.Find(playerCamera.Value).GetComponent<Camera>().GetUniversalAdditionalCameraData();

            if (!targetCamera.cameraStack.Contains(uiCamera))
            {
                targetCamera.cameraStack.Add(uiCamera);
                targetCamera.cameraStack.Reverse();
            }
        }
    }
}
#endif