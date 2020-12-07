#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class DetectionBehavior : EntityVisionData
    {
        [SerializeField] public LayerMask detectionMask;
        [HideInInspector] public Transform playerHead;
        private bool detectedPlayer = false; 
        private RaycastHit hitInfo;

        new void Awake()
        {
            base.Awake();
            playerHead = GameObject.Find("HeadCollider").transform;
            //pingFrequency = 10;
        }

        private void Update()
        {
            if (poweredOn)
            {
                framesPassed++;
                if (framesPassed % pingFrequency == 0)
                {
                    PingForPlayer();
                    framesPassed = 0;
                }
            }

            else return;
        }

        // check if the player is in range 
        void PingForPlayer()
        {
            myPos.x = transform.position.x;
            myPos.z = transform.position.z;

            targetPos.x = playerHead.transform.position.x;
            targetPos.z = playerHead.transform.position.z;

            myFinalPos.x = transform.position.x;
            myFinalPos.y = playerHead.transform.position.y;
            myFinalPos.z = transform.position.z;

            sqrDistToTarget = (targetPos - myPos).sqrMagnitude;

            // if the player is within the vision range
            if (sqrDistToTarget < rangeOfVision * rangeOfVision)
            {
                // get the direction of the player's head...
                targetDir = playerHead.position - myFinalPos;

                //...if the angle between the looking dir of the cam and the player is less than the cone of vision, then you are inside the cone of vision
                if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f && !detectedPlayer) 
                    LineOfSightCheck();
            }
        }

        private void LineOfSightCheck()
        {
            // if you hit something between the camera and the player's head position
            if (Physics.Linecast(transform.position, playerHead.position, out hitInfo, detectionMask))
            {
                if (hitInfo.collider.gameObject.name == playerHead.name)
                {
                    Debug.Log(gameObject.name + " spotted the player");
                    awarenessManager.RaiseAlarm(entityType, EntityType.Player);
                    detectedPlayer = true;
                }

                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    Debug.DrawLine(transform.position, playerHead.position, Color.red);
                    Debug.Log("I hit " + hitInfo.collider.gameObject.name);
                }
            }
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            if (awarenessManager.alarmRaisers.Contains(entityType)) 
                awarenessManager.alarmRaisers.Remove(entityType);
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
