using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class OxygenVRModifier : Modifier
    {
        public float depletionTime;
        private float currentDepletionTime;

        public CallableFunction gameOver;

        public LightManager lightManager;
        public Color alarmLightColor;
        public float alarmTime;
        private float currentAlarmTime;
        private bool isAlarm;

        public override void Init()
        {
            currentDepletionTime = depletionTime;

            currentAlarmTime = alarmTime;
            isAlarm = true;

            base.Init();
        }

        void Update()
        {
            if (active)
            {
                if (currentDepletionTime <= 0.0f) gameOver.Raise();

                currentDepletionTime -= Time.deltaTime;

                Alarm();
            }
        }

        private void Alarm()
        {
            if (currentAlarmTime <= 0.0f)
            {
                foreach (Light light in lightManager.lights)
                {
                    light.color = isAlarm ? alarmLightColor : lightManager.baseLightColor;
                }
                currentAlarmTime = alarmTime;

                isAlarm = !isAlarm;
            }

            currentAlarmTime -= Time.deltaTime;
        }
    } 
}
