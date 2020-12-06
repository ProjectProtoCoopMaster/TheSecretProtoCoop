#if UNITY_STANDALONE
using Gameplay.VR.Player;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class AwarenessManager : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("Alarm Raising")] internal List<EntityVisionData> alarmRaisers = new List<EntityVisionData>();
        
        [SerializeField] internal List<GameObject> deadGuards = new List<GameObject>();
        [SerializeField] [FoldoutGroup("Alarm Raising")] float alarmRaiseDuration;
        [SerializeField] [FoldoutGroup("Debugging")] float timePassed = 0f;

        [SerializeField] [Tooltip("Time will slow down by x amount when the player is detected.")] [FoldoutGroup("Slow Motion")] float reflexModeMultiplier;

        [SerializeField] [FoldoutGroup("Alarm Raising")] bool raisingAlarm = false, changeTime = false;
        [SerializeField] [FoldoutGroup("Alarm Raising")] CallableFunction gameOver;

        TeleportManager player;

        void Awake()
        {
            player = FindObjectOfType<TeleportManager>();
            Time.timeScale = 1f;
        }

        // called by Detection Behaviour
        internal void RaiseAlarm(EntityVisionData alarmRaiser)
        {
            // if the player was spotted by a camera, it's instant gameOver
            if(alarmRaiser.entityType == EntityType.Camera) gameOver.Raise();

            // if the player was spotted by a guard, start the countdown
            if (alarmRaiser.entityType == EntityType.Guard)
            {
                alarmRaisers.Add(alarmRaiser);

                if (raisingAlarm != true)
                {
                    raisingAlarm = true;
                    changeTime = true;
                }
                else return;
            }
        }

        private void Update()
        {
            // wait for player to stop teleporting to activate slow motion mode
            if (changeTime && player.isTeleporting)
            {
                Time.timeScale /= reflexModeMultiplier;
                changeTime = false;
            }

            if (raisingAlarm)
                timePassed += Time.unscaledDeltaTime;

            if (raisingAlarm && alarmRaisers.Count == 0 || timePassed >= alarmRaiseDuration)
            {
                // if there are still entities raising the alarm, it's game over
                if (alarmRaisers.Count > 0)
                {
                    // otherwise, set the world back in order
                    Time.timeScale *= reflexModeMultiplier;

                    raisingAlarm = false;
                    timePassed = 0f;

                    gameOver.Raise();
                }

            }
        }

        public void GE_LoadNewLevel()
        {
            deadGuards = new List<GameObject>();
        }
    }
}
#endif