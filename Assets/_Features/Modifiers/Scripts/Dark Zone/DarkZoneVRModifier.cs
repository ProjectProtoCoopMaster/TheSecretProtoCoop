using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class DarkZoneVRModifier : Modifier
    {
        public GameEvent startShakeMobile;
        public BoolVariable shakeCheck;

        public LightManager lightManager;

        public Light playerNightVisionLight;

        public Transform playerHead, playerHandController;
        public float distance;

        private bool completed;

        public float maxTimeUntilDark, minTimeUntilDark;

        private float timeUntilDark;
        private float currentTime;

        public override void Init()
        {
            lightManager.SetLights(false);

            timeUntilDark = Random.Range(minTimeUntilDark, maxTimeUntilDark);
            currentTime = timeUntilDark;

            VisionOn();

            base.Init();
        }

        public void VisionOff()
        {
            check = true;
            playerNightVisionLight.gameObject.SetActive(false);

            startShakeMobile.Raise();
        }

        public void VisionOn()
        {
            check = false;
            playerNightVisionLight.gameObject.SetActive(true);
        }

        public override void End()
        {
            lightManager.SetLights(true);

            VisionOff();

            base.End();
        }

        void Update()
        {
            if (active)
            {
                if (currentTime <= 0.0f) VisionOff();

                currentTime -= Time.deltaTime;

                if (check) Check();
            }
        }

        private void Check()
        {
            completed = VRCheck() && shakeCheck.Value;

            if (completed)
            {
                VisionOn();

                timeUntilDark = Random.Range(minTimeUntilDark, maxTimeUntilDark);
                currentTime = timeUntilDark;
            }
        }

        private bool VRCheck()
        {
            return Vector3.Magnitude(playerHead.position - playerHandController.position) <= distance;
        }
    }
}
