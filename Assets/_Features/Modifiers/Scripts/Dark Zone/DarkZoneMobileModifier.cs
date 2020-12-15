using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class DarkZoneMobileModifier : Modifier
    {
        public BoolVariable shake;
        public CallableFunction shakeResult;

        public ShakeDetection shakeDetection;

        void Update()
        {
            if (active)
            {
                if (check)
                {
                    MobileCheck(out shake.Value);

                    shakeResult.Raise();
                }
            }
        }

        private void MobileCheck(out bool complete)
        {
            bool shakeDetected;

            shakeDetection.DetectShake(out shakeDetected);

            complete = shakeDetected;
        }

        public void StartShakeCheck()
        {
            shakeDetection.StartShake();
        }
    }
}
