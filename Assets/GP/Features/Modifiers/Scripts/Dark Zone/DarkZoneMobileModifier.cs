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

        //private void Start()
        //{
        //    Init();
        //    check = true;
        //}

        void Update()
        {
            if (active)
            {
                if (check)
                {
                    MobileCheck(out shake.Value);

                    shakeResult.Raise(shake.Value);
                    shake.Value = false;

                }

            }
        }

        private void MobileCheck(out bool complete)
        {
            bool shakeDetected;

            shakeDetection.DetectShake(out shakeDetected);

            complete = shakeDetected;

            if (shakeDetected) check = false;
        }

        public void StartShakeCheck()
        {
            check = true;
            shakeDetection.StartShake();
        }

        private void OnDisable()
        {
            shake.Value = false;
        }
    }
}
