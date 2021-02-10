#if UNITY_STANDALONE
using Gameplay.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : VisionBehavior
    {
        [SerializeField] AnimationManager animationManager;

        [SerializeField] [FoldoutGroup("Debugging")] bool bIsGuard;
        Vector3Variable playerHead, playerHandLeft, playerHandRight;

        public override void Ping()
        {
            if (CanSeeTarget(playerHead.Value) && CanSeeTarget(playerHead.Value) && CanSeeTarget(playerHead.Value))
            {
                detected = true; // stop the detection from looping

                detectionFeedback.PlayDetectionFeedback();

                if (!awarenessManager.alarmRaisers.Contains(this.gameObject))
                {
                    awarenessManager.alarmRaisers.Add(this.gameObject);

                    if (bIsGuard)
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

        /*
        // check if the player is in range 
        public void Ping2()
        {
            myPos.x = transform.position.x;
            myPos.z = transform.position.z;

            targetPosition.x = playerHead.Value.x;
            targetPosition.z = playerHead.Value.z;

            visionPosition.x = transform.position.x;
            visionPosition.y = playerHead.Value.y;
            visionPosition.z = transform.position.z;

            sqrDistToTarget = (targetPosition - myPos).sqrMagnitude;

            // if the player is within the vision range
            if (sqrDistToTarget < rangeOfVision * rangeOfVision)
            {
                // get the direction of the player's head...
                targetDir = playerHead.Value - visionPosition;

                //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f && detectedPlayer == false)
                {
                    // check to see if you can see head
                    if (LineOfSightCheck(playerHead.Value))
                    {
                        // if you can see both hands, then the player has been spotted
                        if (LineOfSightCheck(playerHandLeft.Value) == true && LineOfSightCheck(playerHandRight.Value) == true)
                        {
                            detectedPlayer = true; // stop the detection from looping

                            detectionFeedback.PlayDetectionFeedback();

                            if (!awarenessManager.alarmRaisers.Contains(this.gameObject))
                            {
                                awarenessManager.alarmRaisers.Add(this.gameObject);

                                if (isGuard)
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
                }
            }
        }

        private bool LineOfSightCheck(Vector3 checkPosition)
        {
            // if you hit something between the camera and the player's head position
            if (Physics.Linecast(transform.position, checkPosition, out hitInfo, detectionMask))
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player")) return true;
                else return false;
            }
            else return false;
        }
        */

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            // if you were detecting the player, remove this object from the list of alarm raisers
            if (awarenessManager.alarmRaisers.Contains(this.gameObject))
                awarenessManager.alarmRaisers.Remove(this.gameObject);

            enabled = false;
        }

        /*   #region Mobile Camera Power
        // called from VR_CameraBehavior
        public void UE_DetectionOn()
        {
            updating = true;
        }

        public void UE_DetectionOff()
        {
            updating = false;
        }
        #endregion*/
    }
}
#endif
