#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(VisionData))]
    public abstract class VisionBehavior : MonoBehaviour
    {
        // a reference to the interface
        public VisionData visionData;

        // need to be identical across both Detection and Overwatch
        public float rangeOfVision { get => visionData.rangeOfVision; set => visionData.rangeOfVision = value; }
        public float coneOfVision { get => visionData.coneOfVision; set => visionData.coneOfVision = value; }

        // cached cone of vision/detection values
        protected Vector3 targetDir;
        protected Vector3 myPos, targetPosition, visionPosition;
        internal float sqrDistToTarget;

        protected GameEvent spottedPlayer { get => visionData.spottedPlayer; }
        protected GameEvent spottedDeadBody { get => visionData.spottedPlayer; }
        protected GameEvent playerPeeking { get => visionData.spottedPlayer; }

        protected CallableFunction raiseAlarm2;
        protected StringVariable loseReason;
        protected AlertManager awarenessManager = null;

        [SerializeField] protected DetectionFeedback detectionFeedback;

        [SerializeField] protected EntityType entityType;
        [SerializeField] LayerMask visionLayerMask;
        [SerializeField] LayerMask targetLayerMask;
        RaycastHit hitInfo;

        // has the entity detected a target ? 
        [ReadOnly] protected bool detected;

        // used to update the entity's update every X frames
        [ReadOnly] protected int pingFrequency;
        [ReadOnly] protected int framesPassed;

        // used to know if the entity of type Camera is active
        [ReadOnly] internal bool updating;

        protected void Awake() => awarenessManager = FindObjectOfType<AlertManager>();

        private void OnEnable() => updating = true;

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
                    if (Physics.Linecast(transform.position, targetPosition, out hitInfo, visionLayerMask))
                    {
                        if (hitInfo.collider.gameObject.layer == targetLayerMask) return true;
                        else return false;
                    }
                    else return true;
                }
                else return false;
            }
            else return false;
        }

        public void GE_RefreshScene() => detected = false;

        private void OnDisable() => updating = false;
    }
}
#endif