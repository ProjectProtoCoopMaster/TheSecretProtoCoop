#if UNITY_STANDALONE
using Gameplay.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : VisionBehavior
    {
        [SerializeField] AnimationManager animationManager;

        Vector3Variable playerHead, playerHandLeft, playerHandRight;

        public override void Ping()
        {
            if (CanSeeTarget(playerHead.Value) && CanSeeTarget(playerHandLeft.Value) && CanSeeTarget(playerHandRight.Value))
            {
                detected = true; // stop the detection from looping

                detectionFeedback.PlayDetectionFeedback();

                if (!awarenessManager.alarmRaisers.Contains(this.gameObject))
                {
                    awarenessManager.alarmRaisers.Add(this.gameObject);

                    if (entityType == EntityType.Guard)
                    {
                        // Animation, Léonard kiffe bien à réecrire ça bruuuh
                        animationManager.SetAlertAnim();
                        GetComponent<AgentManager>().StopAgent();
                    }
                }

                spottedPlayer.Raise();
                Debug.Log(gameObject.name + " spotted the player !");
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
            if (awarenessManager.alarmRaisers.Contains(this.gameObject))
                awarenessManager.alarmRaisers.Remove(this.gameObject);

            enabled = false;
        }
    }
}
#endif
