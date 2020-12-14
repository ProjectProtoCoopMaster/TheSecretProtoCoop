
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Gameplay.GameManager;

namespace Gameplay.VR
{
    public class AwarenessManager : MonoBehaviour
    {
#if UNITY_STANDALONE
        [SerializeField] [FoldoutGroup("Slow Motion")] GameEvent reflexModeOn;
        [SerializeField] [FoldoutGroup("Slow Motion")] GameEvent reflexModeOff;

        [SerializeField] [FoldoutGroup("Alarm Raising")] bool raisingAlarm = false, spottedPlayer = false, spottedDeadBody = false;
        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal int alarmRaisers;
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<Transform> deadGuards = new List<Transform>();

        [SerializeField] [FoldoutGroup("Alarm Raising")] CallableFunction gameOver;
        [SerializeField] [FoldoutGroup("Alarm Raising")] GameEvent gameOverAlarm;

        bool gameIsOver;

        [SerializeField] [FoldoutGroup("Debugging")] float timePassed = 0f;

        private void Awake()
        {
            Time.timeScale = 1f;
            alarmRaisers = 0;
        }

        #region Game Events
        // called every time a guard is killed
        public void GE_AlarmRaiserKilled()
        {
            if(alarmRaisers > 0) alarmRaisers--;
        }

        // called when the player is detected by a guard
        public void GE_PlayerDetectedByGuard()
        {
            alarmRaisers++;
            spottedPlayer = true;

            if (raisingAlarm != true)
            {
                raisingAlarm = true; // prevent the event from being raised more than once
                reflexModeOn.Raise();
            }
        }

        public void GE_PlayerDetectedByCamera()
        {
            GameOver(LoseType.PlayerSpottedByCam);
        }

        public void GE_BodyDetectedByGuard()
        {
            alarmRaisers++;
            spottedDeadBody = true;

            if (raisingAlarm != true)
            {
                raisingAlarm = true; // prevent the event from being raised more than once
                reflexModeOn.Raise();
            }
        }

        public void GE_BodyDetectedByCamera()
        {
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
                if (alarmRaisers == 0) KillAlarm();

                // if the alarm has passed its limit
                if (timePassed >= alarmRaiseDuration)
                {
                    // if there are still entities raising the alarm, it's game over
                    if (alarmRaisers > 0)
                    {
                        reflexModeOff.Raise();

                        raisingAlarm = false;
                        timePassed = 0f;

                        if (spottedDeadBody) GameOver(LoseType.BodySpottedByGuard);
                        if (spottedPlayer) GameOver(LoseType.PlayerSpottedByGuard);
                    }
                }
            }
        }

        private void KillAlarm()
        {
            raisingAlarm = false;
            reflexModeOff.Raise();

            spottedDeadBody = false;
            spottedPlayer = false;
        }

        private void GameOver(LoseType loseReason)
        {
            if (!gameIsOver)
            {
                gameIsOver = true;
                gameOver.Raise((int)loseReason);
                gameOverAlarm.Raise();
            }
        }
#endif
    }
}