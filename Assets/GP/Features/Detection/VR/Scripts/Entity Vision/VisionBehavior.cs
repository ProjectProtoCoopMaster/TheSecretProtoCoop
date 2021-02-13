#if UNITY_STANDALONE
using Gameplay.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(VisionData))]
    public abstract class VisionBehavior : MonoBehaviour
    {
        [Title("Common")]

        public VisionData visionData;

        [SerializeField] protected DetectionFeedback detectionFeedback;

        [SerializeField] LayerMask visionLayerMask;
        [SerializeField] LayerMask targetLayerMask;
        RaycastHit hitInfo;

        [SerializeField] protected int pingFrequency;

        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly] protected int framesPassed;
        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly] protected bool detected;
        [SerializeField] [FoldoutGroup("Debug")] [ReadOnly] internal bool updating;

        [SerializeField] protected EntityType entityType;
        [ShowIf("entityType", EntityType.Guard)] [Title("Guard")]
        [SerializeField] [ShowIf("entityType", EntityType.Guard)] protected GuardManager guardManager;
        [SerializeField] [ShowIf("entityType", EntityType.Guard)] protected AnimationManager animationManager;

        public float rangeOfVision { get => visionData.rangeOfVision; set => visionData.rangeOfVision = value; }
        public float coneOfVision { get => visionData.coneOfVision; set => visionData.coneOfVision = value; }

        // cached cone of vision/detection values
        protected Vector3 targetDir;
        protected Vector3 myPos, targetPosition, visionPosition;
        internal float sqrDistToTarget;

        protected GameEvent playerPeeking { get => visionData.playerPeeking; }

        protected AlertManager alertManager { get => GameManager.instance.alertManager; }

        private void Update()
        {
            if (updating)
            {
                framesPassed++;
                if (framesPassed % pingFrequency == 0)
                {
                    Ping();
                    framesPassed = 0;
                }
            }
        }

        // Called every couple of frames to save on performance. Determines who the target is.
        public abstract void Ping();

        // Called by Ping() under certain conditions. Checks if a target is in range.
        public bool CanSeeTarget(Vector3 targetPosition)
        {
            visionPosition.x = transform.position.x;
            visionPosition.y = targetPosition.y;
            visionPosition.z = transform.position.z;

            sqrDistToTarget = (targetPosition - visionPosition).sqrMagnitude;

            // if the player is within the vision range
            if (sqrDistToTarget < rangeOfVision * rangeOfVision)
            {
                // get the direction of the player's head...
                targetDir = targetPosition - visionPosition;

                //...if the angle between the looking dir of the entity and a target element is less than the cone of vision, then you can see him
                if (Vector3.Angle(targetDir, transform.forward) <= coneOfVision * 0.5f && detected == false)
                {
                    if (Physics.Linecast(visionPosition, targetPosition, out hitInfo, visionLayerMask))
                    {
                        // check if the hitObject is on the target LayerMask
                        if ((targetLayerMask.value & (1 << hitInfo.collider.gameObject.layer)) > 0)
                        {
                            Debug.DrawLine(transform.position, targetPosition, Color.green, 1f);
                            return true;
                        }

                        else
                        {
                            Debug.DrawLine(transform.position, targetPosition, Color.red, 1f);
                            return false;
                        }
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public virtual void UE_GuardDied()
        {
            // if you were detecting the player, remove this object from the list of alarm raisers
            if (alertManager.alarmRaisers.Contains(guardManager))
                alertManager.alarmRaisers.Remove(guardManager);

            updating = false;
            detected = false;
        }

        public void GE_RefreshScene()
        {
            updating = (entityType == EntityType.Guard) ? true : updating;
            detected = false;
        }
    }
}
#endif