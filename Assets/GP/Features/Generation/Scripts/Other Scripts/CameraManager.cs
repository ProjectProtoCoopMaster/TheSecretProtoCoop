using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class CameraManager : MonoBehaviour
    {
        public Camera _camera;

        public float baseOffset;

        public CameraScrollBehaviour cameraScroll;

        public void SetCamera(float width, float height, Transform center)
        {
            Vector3 camPosition = new Vector3(center.position.x, baseOffset, center.position.z);
            transform.position = camPosition;

            cameraScroll.basePosition = new Vector2(center.position.x, center.position.z);

            Vector2 clamp = new Vector2(width, height);
            cameraScroll.clampValue = clamp;
        }
    } 
}
