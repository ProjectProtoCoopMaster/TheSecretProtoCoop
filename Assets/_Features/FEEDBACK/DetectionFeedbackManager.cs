using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR.Feedbacks
{
    public class DetectionFeedbackManager : MonoBehaviour
    {
        [SerializeField] Text peakingText;
        private bool temp_refresh;
        int framesPassed = 0;
        int framesToWait = 20;

        private void Awake()
        {
            GE_RefreshScene();
        }

        public void GE_PlayerPeeking()
        {
            peakingText.enabled = true;
            peakingText.color = Color.yellow;
            peakingText.text = ">Peaking<";
            temp_refresh = true;
            framesPassed = 0;
            framesToWait = 10;
        }

        public void GE_EngageReflexMode()
        {
            peakingText.enabled = true;
            peakingText.color = Color.red;
            peakingText.text = ">Spotted<";
            temp_refresh = false;
        }

        public void GE_DisenageReflexMode()
        {
            peakingText.color = Color.green;
            peakingText.text = ">Clear<";
            temp_refresh = true;
            framesPassed = 0;
            framesToWait = 120;
        }

        void Update()
        {
            if(temp_refresh)
            {
                framesPassed++;

                if (framesPassed % framesToWait == 0)
                    peakingText.enabled = false;
            }
        }

        public void GE_RefreshScene()
        {
            peakingText.enabled = false;
        }
    }
}