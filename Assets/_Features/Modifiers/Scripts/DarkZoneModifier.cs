using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class DarkZoneModifier : Modifier
    {
        public LightManager lightManager;

        public Light playerNightVisionLight;

        public ShakeDetection shakeDetection;
        private bool shakeDetected;

        public override void Init()
        {
            active = true;
            lightManager.SetLights(false);
        }

        public override void Activate()
        {
            check = true;
            playerNightVisionLight.enabled = false;

            shakeDetection.Restart();
        }

        public override void Deactivate()
        {
            check = false;
            playerNightVisionLight.enabled = true;
        }

        void Update()
        {
            /// Check Input / Controller Position from VR Player /// Check Input / Phone Shake from Mobile Player
            if (check)
            {
                shakeDetection.DetectShake(out shakeDetected);

                if (shakeDetected) Deactivate();
            }
        }
    } 
}
