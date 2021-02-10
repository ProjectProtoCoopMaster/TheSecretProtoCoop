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
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<Transform> deadGuards = new List<Transform>();
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<GameObject> alarmRaisers = new List<GameObject>();

        [SerializeField] [FoldoutGroup("Alarm Raising")] GameEvent gameOverAlarm;

        [SerializeField] [FoldoutGroup("Debugging")] float timePassed;

        #region Game Events
        public void Alert()
        {
            if (!alert)
            {
                // Call the Alert
            }
            alert = true;
        }
        public void Detected()
        {

        }

        public void Incognito()
        {
            alert = false;
            // Call Incognito
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
                        if (spottedDeadBody)
                        {
                            Debug.Log("Restarting the game, a guard saw a body");
                            GameOver(LoseType.BodySpottedByGuard);
                        }
                        if (spottedPlayer)
                        {
                            Debug.Log("Restarting the game, a guard saw me");
                            GameOver(LoseType.PlayerSpottedByGuard);
                        }

                        GE_RefreshScene();
                    }
                }
            }
        }

        private void GameOver(LoseType loseType)
        {
            TransmitterManager.instance.SendLoseToAll((int)loseType);
            gameOverAlarm.Raise();
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