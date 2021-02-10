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

        [SerializeField] [FoldoutGroup("Player Detection State")] GameEvent playerSpotted, playerIncognito;

        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;

        [ReadOnly] [FoldoutGroup("Alarm Raising")]
        internal List<GuardManager> deadGuards = new List<GuardManager>();
        [ReadOnly] [FoldoutGroup("Alarm Raising")]
        internal List<GuardManager> alarmRaisers = new List<GuardManager>();

        [SerializeField] [FoldoutGroup("Alarm Raising")] GameEvent gameOverAlarm;

        [SerializeField] [FoldoutGroup("Debugging")] float timePassed;

        public LoseType loseType { get; set; }

        #region Game Events
        public void Alert()
        {
            if (!alert)
            {
                // Call the Alert Feedback Text
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
            timePassed = 0.0f;
        }

        public void GE_PlayerDetectedByGuard()
        {
            if (raisingAlarm != true)
            {
                raisingAlarm = true; // prevent the event from being raised more than once
                playerSpotted.Raise();
            }
        }

        public void GE_PlayerDetectedByCamera()
        {
            Debug.Log("Restarting the game, a camera saw me");
            GameOver(LoseType.PlayerSpottedByCam);
        }

        public void GE_BodyDetectedByGuard()
        {
            if (raisingAlarm != true)
            {
                raisingAlarm = true; // prevent the event from being raised more than once
                playerSpotted.Raise();
            }
        }

        public void GE_BodyDetectedByCamera()
        {
            Debug.Log("Restarting the game, a camera saw a body");
            GameOver(LoseType.BodySpottedByCam);
        }
        #endregion

        private void Update()
        {
            if (alert)
            {
                // increase the time that is currently passing
                timePassed += Time.unscaledDeltaTime;

                // if you kill all the alarm raising entities
                if (alarmRaisers.Count == 0) Incognito();

                // if the alarm has passed its limit
                if (timePassed >= alarmRaiseDuration)
                {
                    // if there are still entities raising the alarm, it's game over
                    if (alarmRaisers.Count > 0)
                    {
                        Detected();

                        GE_RefreshScene();
                    }
                }
            }
        }

        public void GE_RefreshScene()
        {
            alert = false;

            deadGuards.Clear();
            alarmRaisers.Clear();

            timePassed = 0f;
            Time.timeScale = 1f;
        }
#endif
    }
}