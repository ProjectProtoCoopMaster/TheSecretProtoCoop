#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : VisionBehavior
    {
        public override void Ping()
        {
            for (int i = 0; i < alertManager.deadGuards.Count; i++)
            {
                if (CanSeeTarget(alertManager.deadGuards[i].transform.position))
                {
                    detected = true;

                    detectionFeedback.PlayDetectionFeedback();

                    if (!alertManager.alarmRaisers.Contains(guardManager))
                        alertManager.alarmRaisers.Add(guardManager);

                    if (entityType == EntityType.Guard)
                    {
                        alertManager.loseType = LoseType.BodySpottedByGuard;
                        alertManager.Alert();

                        animationManager.SetAlertAnim();
                        guardManager.StopAgent();
                    }

                    else if (entityType == EntityType.Camera)
                    {
                        alertManager.loseType = LoseType.BodySpottedByCam;
                        alertManager.Detected();
                    }
                }
                else continue;
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            // if you were detecting the player, remove this object from the list of alarm raisers
            if (alertManager.alarmRaisers.Contains(guardManager))
                alertManager.alarmRaisers.Remove(guardManager);

            // add this entity to the list of dead guards
            if (!alertManager.deadGuards.Contains(guardManager))
                alertManager.deadGuards.Add(guardManager);

            enabled = false;
        }
    }
}
#endif