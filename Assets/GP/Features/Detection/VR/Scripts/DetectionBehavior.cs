#if UNITY_STANDALONE
using Gameplay.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : VisionBehavior
    {
        [SerializeField] Vector3Variable playerHead, playerHandLeft, playerHandRight;

        public override void Ping()
        {
            if (CanSeeTarget(playerHead.Value) && CanSeeTarget(playerHandLeft.Value) && CanSeeTarget(playerHandRight.Value))
            {
                detected = true; // stop the detection from looping

                Debug.Log("player is detected !");

                detectionFeedback.PlayDetectionFeedback();

                if (!alertManager.alarmRaisers.Contains(guardManager))
                    alertManager.alarmRaisers.Add(guardManager);

                if (entityType == EntityType.Guard)
                {
                    alertManager.loseType = LoseType.PlayerSpottedByGuard;
                    alertManager.Alert();

                    animationManager.SetAlertAnim();
                    guardManager.StopAgent();
                }

                else if (entityType == EntityType.Camera)
                {
                    alertManager.loseType = LoseType.PlayerSpottedByCam;
                    alertManager.Detected();
                }
            }

            //...otherwise, it means that the player is "peeking"
            else
            {
                playerPeeking.Raise();
                Debug.Log("The player is peeking !");
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            // if you were detecting the player, remove this object from the list of alarm raisers
            if (alertManager.alarmRaisers.Contains(guardManager))
                alertManager.alarmRaisers.Remove(guardManager);

            enabled = false;
        }
    }
}
#endif
