#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR
{
    public class OverwatchBehavior : EntityVisionData
    {
        [SerializeField] Transform myDetectableBody;
        [SerializeField] public LayerMask overwatchMask;
        private RaycastHit hitInfo;
        bool detectedGuard = false;

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
                    if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f && !detectedGuard)
                    {
                        if (Physics.Linecast(this.transform.position, awarenessManager.deadGuards[i].position, out hitInfo, overwatchMask))
                        {
                            if (hitInfo.collider.gameObject.CompareTag("Dead"))
                            {
                                raiseAlarm.Raise();
                                //awarenessManager.RaiseAlarm(entityType, EntityType.Guard);
                                detectedGuard = true;
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
            if (awarenessManager.alarmRaisers.Contains(entityType)) 
                awarenessManager.alarmRaisers.Remove(entityType);
            
            awarenessManager.deadGuards.Add(myDetectableBody);

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
    }
} 
#endif