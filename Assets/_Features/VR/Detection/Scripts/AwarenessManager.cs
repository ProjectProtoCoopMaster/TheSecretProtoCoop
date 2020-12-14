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
        [SerializeField] [FoldoutGroup("Slow Motion")] GameEvent reflexModeOn;
        [SerializeField] [FoldoutGroup("Slow Motion")] GameEvent reflexModeOff;

        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<Transform> deadGuards = new List<Transform>();
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<GameObject> alarmRaisers = new List<GameObject>();

        [SerializeField] [FoldoutGroup("Alarm Raising")] CallableFunction gameOver;
        [SerializeField] [FoldoutGroup("Alarm Raising")] GameEvent gameOverAlarm;

        [SerializeField] bool raisingAlarm = false, spottedPlayer = false, spottedDeadBody = false, gameIsOver = false;
        [SerializeField] [FoldoutGroup("Debugging")] float timePassed = 0f;
        //[SerializeField] [FoldoutGroup("Debugging")] internal int alarmRaisers;

        private void Awake()
        {
            GE_RefreshScene();
        }

        #region Game Events
        // called when the player is detected by a guard
        public void GE_PlayerDetectedByGuard()
        {
            spottedPlayer = true;

            if (raisingAlarm != true)
            {
                raisingAlarm = true; // prevent the event from being raised more than once
                reflexModeOn.Raise();
            }
        }

        public void GE_PlayerDetectedByCamera()
        {
            Debug.Log("Restarting the game, a camera saw me");
            GameOver(Gameplay.GameManager.LoseType.PlayerSpottedByCam);
        }

        public void GE_BodyDetectedByGuard()
        {
            spottedDeadBody = true;

            if (raisingAlarm != true)
            {
                raisingAlarm = true; // prevent the event from being raised more than once
                reflexModeOn.Raise();
            }
        }

        public void GE_BodyDetectedByCamera()
        {
            Debug.Log("Restarting the game, a camera saw a body");
            GameOver(Gameplay.GameManager.LoseType.BodySpottedByCam);
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
                        reflexModeOff.Raise();

                        if (spottedDeadBody)
                        {
                            Debug.Log("Restarting the game, a guard saw a body");
                            GameOver(Gameplay.GameManager.LoseType.BodySpottedByGuard);
                        }
                        if (spottedPlayer)
                        {
                            Debug.Log("Restarting the game, a guard saw me");
                            GameOver(Gameplay.GameManager.LoseType.PlayerSpottedByGuard);
                        }

                        GE_RefreshScene();
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

            timePassed = 0f;
        }

        private void GameOver(Gameplay.GameManager.LoseType loseReason)
        {
            if (!gameIsOver)
            {
                gameIsOver = true;
            }
            gameOver.Raise((int)loseReason);
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