using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : EntityVisionDataInterface
    {
        private bool isDetected = false;

        private void Start()
        {
            poweredOn = true;
            StartCoroutine(PlayerInRangeCheck());
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            enabled = false;
        }

        #region Mobile Camera Power
        // called from VR_CameraBehavior
        public void DetectionOn()
        {
            poweredOn = true;
            StartCoroutine(PlayerInRangeCheck());
        }
        public void DetectionOff()
        {
            poweredOn = false;
            StopAllCoroutines();
        } 
        #endregion

        // check if the player is in range 
        IEnumerator PlayerInRangeCheck()
        {
            while (true)
            {
                if (!poweredOn) break;

                myPos.x = transform.position.x;
                myPos.z = transform.position.z;

                targetPos.x = playerHead.transform.position.x;
                targetPos.z = playerHead.transform.position.z;

                myFinalPos.x = transform.position.x;
                myFinalPos.y = playerHead.transform.position.y;
                myFinalPos.z = transform.position.z;

                sqrDistToTarget = (targetPos - myPos).sqrMagnitude;
                // Debug.DrawLine(transform.position, playerHead.position, Color.white);

                // if the player is within the vision range
                if (sqrDistToTarget < rangeOfVision * rangeOfVision)
                {
                    // get the direction of the player's head...
                   targetDir = playerHead.position - myFinalPos;
                   // Vector3 targetDir = playerHead.position - transform.position;
                    
                    //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                    if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f) 
                        PlayerInSightCheck();
                }

                yield return null;
            }
        }

        // if the player is in range and in the cone of vision, check if you have line of sight to his head collider
        void PlayerInSightCheck()
        {
            Debug.Log("I'm checking" + transform.name);

            // if you hit something between the camera and the player's head position
            if (Physics.Linecast(transform.position, playerHead.position, out hitInfo, detectionMask))
            {
                if (hitInfo.collider.gameObject.name == playerHead.name)
                {
                    if (!isDetected)
                    {
                        Debug.Log(gameObject.name + " spotted the player");
                        awarenessManager.RaiseAlarm();
                        isDetected = true;
                    }
                }

                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    Debug.DrawLine(transform.position, playerHead.position, Color.red);
                    Debug.Log("I hit " + hitInfo.collider.gameObject.name);
                }
            }
            else if (!Physics.Linecast(transform.position, playerHead.position, out hitInfo, detectionMask))
                Debug.Log("No Collisions");
        }
    }
}
