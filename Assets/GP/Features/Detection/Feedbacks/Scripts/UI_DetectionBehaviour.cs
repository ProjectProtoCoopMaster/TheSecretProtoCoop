using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.VR.UI
{
    public class UI_DetectionBehaviour : MonoBehaviour
    {
        [SerializeField] TMP_Text detectionText;
        private bool decay;
        int framesPassed = 0;
        int framesToWait = 20;

        private void Awake()
        {
            //GE_RefreshScene();
            GE_DisenageReflexMode();
        }

        public void GE_PlayerPeeking()
        {
            detectionText.enabled = true;
            detectionText.color = Color.yellow;
            detectionText.text = "[Peaking]";
            decay = true;
            framesPassed = 0;
            framesToWait = 10;
        }

        public void GE_EngageReflexMode()
        {
            detectionText.enabled = true;
            detectionText.color = Color.red;
            detectionText.text = "[Danger]";
            decay = false;
        }

        public void GE_DisenageReflexMode()
        {
            detectionText.color = Color.green;
            detectionText.text = "[Incognito]";
            framesPassed = 0;
            framesToWait = 120;
        }

        public void GE_GameOver()
        {
            detectionText.enabled = false;
        }

        void Update()
        {
            if (decay)
            {
                framesPassed++;

                if (framesPassed % framesToWait == 0)
                {
                    GE_DisenageReflexMode();
                }
            }
        }

        public void GE_RefreshScene()
        {
            detectionText.enabled = false;
        }
    }
}