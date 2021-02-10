using Gameplay.AI;
using Networking;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    public class AlertManager : MonoBehaviour
    {
#if UNITY_STANDALONE
        public bool alert { get; set; }

        [SerializeField] GameEvent playerSpotted;
        [SerializeField] GameEvent playerIncognito;

        [SerializeField] GameEvent gameOverAlarm;

        [SerializeField] float alertDuration;

        [Title("Debug")]
        [ReadOnly] public List<GuardManager> deadGuards = new List<GuardManager>();
        [ReadOnly] public List<GuardManager> alarmRaisers = new List<GuardManager>();

        private float currentTime;

        public LoseType loseType { get; set; }

        public void Alert()
        {
            if (!alert)
            {
                // Call the Alert Feedback Text
                playerSpotted.Raise();
            }
            alert = true;
        }
        public void Detected()
        {
            TransmitterManager.instance.SendLoseToAll((int)loseType);
            gameOverAlarm.Raise();
        }

        public void Incognito()
        {
            alert = false;
            // Call Incognito Feedback Text
            playerIncognito.Raise();
            currentTime = 0.0f;
        }

        private void Update()
        {
            if (alert)
            {
                // increase the time that is currently passing
                currentTime += Time.unscaledDeltaTime;

                // if you kill all the alarm raising entities
                if (alarmRaisers.Count == 0) Incognito();

                // if the alarm has passed its limit
                if (currentTime >= alertDuration)
                {
                    // if there are still entities raising the alarm, it's game over
                    if (alarmRaisers.Count > 0)
                    {
                        Detected();
                    }
                }
            }
        }

        public void _Reset()
        {
            alert = false;

            deadGuards.Clear();
            alarmRaisers.Clear();

            currentTime = 0.0f;
        }
#endif
    }
}
