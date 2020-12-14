using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Feedbacks
{
    public class DetectionFeedbackManager : MonoBehaviour
    {
        public Canvas peakingCanvas;

        private void Awake()
        {
            peakingCanvas.enabled = false;
        }

        public void GE_PlayerPeeking()
        {
            peakingCanvas.enabled = true;
        }

        public void GE_EngageReflexMode()
        {

        }

        public void GE_DisenageReflexMode()
        {

        }
    }
}