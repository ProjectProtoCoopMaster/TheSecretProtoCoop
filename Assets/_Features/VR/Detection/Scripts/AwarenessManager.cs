
using Sirenix.OdinInspector;
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

        [SerializeField] [FoldoutGroup("Alarm Raising")] bool raisingAlarm = false;
        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<EntityType> alarmRaisers = new List<EntityType>();
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<Transform> deadGuards = new List<Transform>();
        bool changeTime = false;

        [SerializeField] [FoldoutGroup("Alarm Raising")] CallableFunction gameOver;
        [SerializeField] [FoldoutGroup("Alarm Raising")] GameEvent gameOverAlarm;

        bool gameIsOver;

        [SerializeField] [FoldoutGroup("Debugging")] float timePassed = 0f;

        GameOverFeedbackManager gameOverFeedbackManager;
        EntityType alarmRaisingEntity;
        EntityType alarmReasonEntity;
        UnityEvent raiseTheAlarm = new UnityEvent();

        void Awake()
        {
            Time.timeScale = 1f;

            gameOverFeedbackManager = transform.parent.GetComponentInChildren<GameOverFeedbackManager>();

            //  this unity event is what sends the information to the UI
            raiseTheAlarm.AddListener(() => gameOverFeedbackManager.UE_GameOverExplanation(alarmRaisingEntity, alarmReasonEntity));
        }

        // called by Detection and Overwatch Behaviours
        internal void RaiseAlarm(EntityType alarmRaiser, EntityType alarmReason)
        {
            alarmRaisingEntity = alarmRaiser;
            alarmReasonEntity = alarmReason;

            // if the player was spotted by a camera, it's instant gameOver
            if (alarmRaiser == EntityType.Camera) GameOver();

            // if the player was spotted by a guard, start the countdown
            if (alarmRaiser == EntityType.Guard)
            {
                alarmRaisers.Add(alarmRaiser);

                if (raisingAlarm != true)
                {
                    reflexModeOn.Raise();
                    raisingAlarm = true; // prevent the event from being raised more than once
                    changeTime = true;
                }
                else return;
            }
        }

        private void Update()
        {
            if (raisingAlarm) timePassed += Time.unscaledDeltaTime;

            if (raisingAlarm && alarmRaisers.Count == 0) 
                reflexModeOff.Raise();

            if (timePassed >= alarmRaiseDuration)
            {
                // if there are still entities raising the alarm, it's game over
                if (alarmRaisers.Count > 0)
                {
                    reflexModeOff.Raise();

                    raisingAlarm = false;
                    timePassed = 0f;

                    GameOver();
                }
            }
        }

        private void GameOver()
        {
            if (!gameIsOver)
            {
                raiseTheAlarm.Invoke();
                gameIsOver = true;
                gameOver.Raise();
                gameOverAlarm.Raise();
            }
        }
#endif
    }
}