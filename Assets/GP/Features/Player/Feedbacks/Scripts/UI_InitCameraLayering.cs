using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gameplay.VR.Feedbacks
{
    public class UI_InitCameraLayering : MonoBehaviour
    {
        Camera uiCamera;
        [SerializeField] StringVariable playerCamera;

        void Awake()
        {
            uiCamera = GetComponentInChildren<Camera>();
        }

        void Start()
        {
            OnLevelLoaded();
        }

        public void OnLevelLoaded()
        {
            var cameraData = GameObject.Find(playerCamera.Value).GetComponent<Camera>().GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
        }
    }
}