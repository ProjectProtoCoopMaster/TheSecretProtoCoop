using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class DarkZoneModifier : Modifier
    {
        public LightManager lightManager;

        public Light playerNightVisionLight;

        public Transform playerHead, playerHandController;
        public float distance;

        public ShakeDetection shakeDetection;
        private bool shakeDetected;

        private bool completed;

        public float maxTimeUntilDark, minTimeUntilDark;

        private float timeUntilDark;
        private float currentTime;

        public override void Init()
        {
            active = true;
            lightManager.SetLights(false);

            timeUntilDark = Random.Range(minTimeUntilDark, maxTimeUntilDark);
            currentTime = timeUntilDark;

            VisionOn();
        }

        public void VisionOff()
        {
            check = true;
            playerNightVisionLight.gameObject.SetActive(false);

            shakeDetection.Restart();
        }

        public void VisionOn()
        {
            check = false;
            playerNightVisionLight.gameObject.SetActive(true);
        }

        public override void End()
        {
            active = false;
            lightManager.SetLights(true);

            VisionOff();
        }

        void Update()
        {
            if (currentTime <= 0.0f) VisionOff();

            currentTime -= Time.deltaTime;

            if (check) Check();
        }

        private void Check()
        {
            MobileCheck(out completed);

            VRCheck(out completed);

            if (completed)
            {
                VisionOn();

                timeUntilDark = Random.Range(minTimeUntilDark, maxTimeUntilDark);
                currentTime = timeUntilDark;
            }
        }

        private void MobileCheck(out bool complete)
        {
            shakeDetection.DetectShake(out shakeDetected);

            complete = shakeDetected;
        }

        private void VRCheck(out bool complete)
        {
            complete = Vector3.Magnitude(playerHead.position - playerHandController.position) <= distance;
        }
    } 
}
