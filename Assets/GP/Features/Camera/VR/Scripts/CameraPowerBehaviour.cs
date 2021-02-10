using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class CameraPowerBehaviour : MonoBehaviour
    {
        [SerializeField] CameraFeedbackBehaviour feedbackBehaviour;
        [SerializeField] VisionBehavior detectionBehaviour, overwatchBehaviour;

        public void PowerOn()
        {
            detectionBehaviour.updating = true;
            overwatchBehaviour.updating = true;
        }

        public void PowerOff()
        {
            detectionBehaviour.updating = false;
            overwatchBehaviour.updating = false;
        }
    } 
}
