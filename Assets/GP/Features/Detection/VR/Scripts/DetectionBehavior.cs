#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : VisionBehavior
    {
        [SerializeField] Vector3Variable playerHead, playerHandLeft, playerHandRight;

        public override void Ping()
        {
            // if you can see the head...
            if (CanSeeTarget(playerHead.Value))
            {
                // ...and you also see the hands...
                if (CanSeeTarget(playerHandLeft.Value) && CanSeeTarget(playerHandRight.Value))
                {
                    detected = true; // stop the detection from looping

                    Debug.Log(gameObject.name + " has spotted a the player !!!");

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
        }
    }
}
#endif
