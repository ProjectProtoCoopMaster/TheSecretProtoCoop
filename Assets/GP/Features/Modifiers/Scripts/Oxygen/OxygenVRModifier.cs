using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class OxygenVRModifier : Modifier
    {
        public float depletionTime;
        private float currentDepletionTime;

        public CallableFunction _gameOver;
        public CallableFunction _sendOxygenTimer;
        public IntVariable _oxygenTimerMobile;
        public IntVariable _oxygenTimerVR;

        public LightManager lightManager;
        public Color alarmLightColor;
        public float alarmTime;
        private float currentAlarmTime;
        private bool isAlarm;

        private void Start()
        {
            Init();
        }

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
                if(_oxygenTimerMobile.Value <= 100 && _oxygenTimerVR.Value <= 100)
                {
                    if (currentDepletionTime <= 0.0f) _gameOver.Raise();

                    currentDepletionTime -= Time.deltaTime;
                    _oxygenTimerVR.Value = (int)currentDepletionTime;
                    _sendOxygenTimer.Raise(_oxygenTimerVR.Value);

                    Alarm();
                }

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

        private void OnDisable()
        {
            _oxygenTimerVR.Value = 0;
            _oxygenTimerMobile.Value = 0;
        }
    }
}
