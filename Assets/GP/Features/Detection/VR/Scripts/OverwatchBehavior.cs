#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : EntityVisionData
    {
        [SerializeField] Transform myDetectableBody;
        [SerializeField] public LayerMask overwatchMask;
        private RaycastHit hitInfo;
        private bool detectedBody = false;

        new void Awake()
        {
            base.Awake();
        }

        public override void Ping()
        {
            myPos.x = transform.position.x;
            myPos.y = transform.position.z;

            for (int i = 0; i < awarenessManager.deadGuards.Count; i++)
            {
                if (awarenessManager.deadGuards[i] != null)
                {
                    targetPos.x = awarenessManager.deadGuards[i].position.x;
                    targetPos.y = awarenessManager.deadGuards[i].position.z;

                    myFinalPos.x = transform.position.x;
                    myFinalPos.y = awarenessManager.deadGuards[i].position.y;
                    myFinalPos.z = transform.position.z;

                    sqrDistToTarget = (targetPos - myPos).sqrMagnitude;

                    // if the target guard is within the vision range
                    if (sqrDistToTarget < rangeOfVision * rangeOfVision)
                    {
                        // get the entity's direction relative to you...
                        targetDir = awarenessManager.deadGuards[i].position - myFinalPos;

                        //...if the angle between the looking dir of the entity and a dead guard is less than the cone of vision, then you can see him
                        if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f && detectedBody == false)
                        {
                            if (Physics.Linecast(this.transform.position, awarenessManager.deadGuards[i].position, out hitInfo, overwatchMask))
                            {
                                if (hitInfo.collider.gameObject.CompareTag("Dead"))
                                {
                                    detectedBody = true; // stop overwatch from looping

                                    detectionFeedback.PlayDetectionFeedback();

                                    Debug.Log(gameObject.name + " is Incrementing the number of Alarm Raisers");

                                    if (!awarenessManager.alarmRaisers.Contains(this.gameObject))
                                        awarenessManager.alarmRaisers.Add(this.gameObject);

                                    spottedDeadBody.Raise();
                                }

                                else if (hitInfo.collider != null) Debug.Log(gameObject.name + "'s Overwatch hit " + hitInfo.collider.GetComponentInParent<AgentDeath>().gameObject.name + "'s " + hitInfo.collider.gameObject.name);
                            }
                        }
                    }
                }
            }

            Debug.LogWarning(gameObject.name + " is checking for dead friendlies");
        }

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            // if you were detecting the player, remove this object from the list of alarm raisers
            if (awarenessManager.alarmRaisers.Contains(this.gameObject))
                awarenessManager.alarmRaisers.Remove(this.gameObject);

            // add this entity to the list of dead guards
            if (!awarenessManager.deadGuards.Contains(myDetectableBody))
                awarenessManager.deadGuards.Add(myDetectableBody);

            // change the object's tags to "Dead"
            myDetectableBody.gameObject.tag = "Dead";

            for (int i = 0; i < myDetectableBody.childCount; i++)
                myDetectableBody.GetChild(i).tag = "Dead";

            enabled = false;
        }

        #region Mobile Camera Power
        // called from VR_CameraBehavior
        public void UE_OverwatchOn()
        {
            poweredOn = true;
        }

        public void UE_OverwatchOff()
        {
            poweredOn = false;
        }
        #endregion

        public void GE_RefreshScene()
        {
            detectedBody = false;
        }
    }
}
#endif