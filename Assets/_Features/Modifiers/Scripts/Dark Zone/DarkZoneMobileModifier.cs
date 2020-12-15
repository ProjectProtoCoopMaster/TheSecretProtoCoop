using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class DarkZoneMobileModifier : Modifier
    {
        public BoolVariable mobileCheck;
        public CallableFunction sendMobileCheck;

        public ShakeDetection shakeDetection;
        private bool shakeDetected;

        void Update()
        {
            if (active)
            {
                if (check)
                {
                    MobileCheck(out mobileCheck.Value);

                    sendMobileCheck.Raise();
                }
            }
        }

        private void MobileCheck(out bool complete)
        {
            shakeDetection.DetectShake(out shakeDetected);

            complete = shakeDetected;
        }

        public void RestartDetection()
        {
            shakeDetection.Restart();
        }
    }
}
