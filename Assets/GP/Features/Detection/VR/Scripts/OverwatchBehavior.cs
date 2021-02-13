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
                    Debug.Log(gameObject.name + " has spotted a dead guard !!!");

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
        public override void UE_GuardDied()
        {
            base.UE_GuardDied();

            // add this entity to the list of dead guards
            if (!alertManager.deadGuards.Contains(guardManager))
                alertManager.deadGuards.Add(guardManager);
        }
    }
}
#endif