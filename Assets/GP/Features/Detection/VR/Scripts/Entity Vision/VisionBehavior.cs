#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        protected Vector3 myPos, targetPos, myFinalPos;
        internal float sqrDistToTarget;

        protected GameEvent spottedPlayer, spottedDeadBody, playerPeeking;
        protected CallableFunction raiseAlarm2;
        protected StringVariable loseReason;
        protected AlertManager awarenessManager = null;

        // used to update the entity's update every X frames
        [SerializeField] [FoldoutGroup("Debugging")] protected int pingFrequency;
        [SerializeField] [FoldoutGroup("Debugging")] protected int framesPassed;

        // used to know if the entity of type Camera is active
        [SerializeField] [FoldoutGroup("Debugging")] protected bool poweredOn;

        protected DetectionFeedback detectionFeedback;

        protected void Awake()
        {
            detectionFeedback = GetComponent<DetectionFeedback>();

            spottedPlayer = visionData.spottedPlayer;
            spottedDeadBody = visionData.spottedDeadBody;
            playerPeeking = visionData.playerPeeking;

            awarenessManager = FindObjectOfType<AlertManager>();
        }

        private void Update()
        {
            if (poweredOn)
            {
                framesPassed++;
                if (framesPassed % pingFrequency == 0)
                {
                    Ping();
                    framesPassed = 0;
                }
            }
        }

        public abstract void Ping();
    }
}
#endif