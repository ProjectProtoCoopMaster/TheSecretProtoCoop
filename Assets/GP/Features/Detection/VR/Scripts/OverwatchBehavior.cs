#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : VisionBehavior
    {
        [SerializeField] internal Transform myDetectableBody;

        public override void Ping()
        {
            for (int i = 0; i < awarenessManager.deadGuards.Count; i++)
            {
                if (CanSeeTarget(awarenessManager.deadGuards[i].position))
                {
                    detected = true;

                    detectionFeedback.PlayDetectionFeedback();

                    if (!awarenessManager.alarmRaisers.Contains(this.gameObject))
                        awarenessManager.alarmRaisers.Add(this.gameObject);

                    spottedDeadBody.Raise();
                }
                else continue;
            }
        }


        /*
        public void Ping2()
        {
            myPos.x = transform.position.x;
            myPos.y = transform.position.z;

            for (int i = 0; i < awarenessManager.deadGuards.Count; i++)
            {
                if (awarenessManager.deadGuards[i] != null)
                {
                    targetPosition.x = awarenessManager.deadGuards[i].position.x;
                    targetPosition.y = awarenessManager.deadGuards[i].position.z;

                    visionPosition.x = transform.position.x;
                    visionPosition.y = awarenessManager.deadGuards[i].position.y;
                    visionPosition.z = transform.position.z;

                    sqrDistToTarget = (targetPosition - myPos).sqrMagnitude;

                    // if the target guard is within the vision range
                    if (sqrDistToTarget < rangeOfVision * rangeOfVision)
                    {
                        // get the entity's direction relative to you...
                        targetDir = awarenessManager.deadGuards[i].position - visionPosition;

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

                                //else if (hitInfo.collider != null) Debug.Log(gameObject.name + "'s Overwatch hit " + hitInfo.collider.GetComponentInParent<AudioSource>().gameObject.name + "'s " + hitInfo.collider.gameObject.name);
                            }
                        }
                    }
                }
            }

            Debug.LogWarning(gameObject.name + " is checking for dead friendlies");
        }*/

        //called by Unity Event when the guard is killed
        public void UE_GuardDied()
        {
            myDetectableBody = GetComponentInChildren<DetectableBodyBehaviour>().transform;

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
            updating = true;
        }

        public void UE_OverwatchOff()
        {
            updating = false;
        }
        #endregion
    }
}
#endif