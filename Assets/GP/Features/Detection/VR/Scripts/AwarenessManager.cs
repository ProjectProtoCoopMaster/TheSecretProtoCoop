using Networking;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    public class AwarenessManager : MonoBehaviour
    {
#if UNITY_STANDALONE
        [SerializeField] [FoldoutGroup("Player Detection State")] GameEvent playerSpotted, playerIncognito;

        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<Transform> deadGuards = new List<Transform>();
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<GameObject> alarmRaisers = new List<GameObject>();

        [SerializeField] [FoldoutGroup("Alarm Raising")] GameEvent gameOverAlarm;

        [SerializeField] bool raisingAlarm = false, spottedPlayer = false, spottedDeadBody = false, gameOver = false;
        [SerializeField] [FoldoutGroup("Debugging")] float timePassed = 0f;

        #region Game Events
        public void GE_PlayerDetectedByGuard()
        {
            spottedPlayer = true;

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
            spottedDeadBody = true;

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
            if (raisingAlarm)
            {
                // increase the time that is currently passing
                timePassed += Time.unscaledDeltaTime;

                // if you kill all the alarm raising entities
                if (alarmRaisers.Count == 0) KillAlarm();

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

        private void KillAlarm()
        {
            raisingAlarm = false;
            playerIncognito.Raise();

            spottedDeadBody = false;
            spottedPlayer = false;

            timePassed = 0f;
        }

        private void GameOver(LoseType loseType)
        {
            if (!gameOver) gameOver = true;
            TransmitterManager.instance.SendLoseToAll((int)loseType);
            gameOverAlarm.Raise();
        }

        public void GE_RefreshScene()
        {
            deadGuards.Clear();
            alarmRaisers.Clear();
            raisingAlarm = false;
            timePassed = 0f;
            Time.timeScale = 1f;
        }
#endif
    }
}