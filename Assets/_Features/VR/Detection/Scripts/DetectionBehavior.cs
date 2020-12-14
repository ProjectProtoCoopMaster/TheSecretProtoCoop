#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : EntityVisionData
    {
        [SerializeField] public LayerMask detectionMask;
        private RaycastHit hitInfo;
        private bool detectedPlayer = false;

        public Vector3Variable playerHead, playerHandLeft, playerHandRight;

        new void Awake()
        {
            base.Awake();
        }

        // check if the player is in range 
        public override void Ping()
        {
            myPos.x = transform.position.x;
            myPos.z = transform.position.z;

            targetPos.x = playerHead.Value.x;
            targetPos.z = playerHead.Value.z;

            myFinalPos.x = transform.position.x;
            myFinalPos.y = playerHead.Value.y;
            myFinalPos.z = transform.position.z;

            sqrDistToTarget = (targetPos - myPos).sqrMagnitude;

            // if the player is within the vision range
            if (sqrDistToTarget < rangeOfVision * rangeOfVision)
            {
                // get the direction of the player's head...
                targetDir = playerHead.Value - myFinalPos;

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
                                awarenessManager.alarmRaisers.Add(this.gameObject);

                            spottedPlayer.Raise();
                            Debug.Log("I spotted the player !");
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

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            // if you were detecting the player, remove this object from the list of alarm raisers
            if (awarenessManager.alarmRaisers.Contains(this.gameObject))
                awarenessManager.alarmRaisers.Remove(this.gameObject);

            enabled = false;
        }

        #region Mobile Camera Power
        // called from VR_CameraBehavior
        public void UE_DetectionOn()
        {
            poweredOn = true;
        }

        public void UE_DetectionOff()
        {
            poweredOn = false;
        }
        #endregion
    }
}
#endif
