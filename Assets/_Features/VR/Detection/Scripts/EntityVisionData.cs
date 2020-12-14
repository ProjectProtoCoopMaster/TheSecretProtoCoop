#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    [RequireComponent(typeof(EntityDataInterface))]
    public abstract class EntityVisionData : MonoBehaviour
    {
        // a reference to the interface
        EntityDataInterface entityDataInterface = null;

        // need to be identical across both Detection and Overwatch
        [HideInInspector] public float rangeOfVision;
        [HideInInspector] public float coneOfVision;

        // cached cone of vision/detection values
        protected Vector3 targetDir;
        protected Vector3 myPos, targetPos, myFinalPos;
        internal float sqrDistToTarget;

        // usued for the Awareness Manager to determine who detected
        internal EntityType entityType;

        protected GameEvent baseState, spottedPlayer, spottedDeadBody, playerPeaking, alarmRaiserKilled;
        protected CallableFunction raiseAlarm2;
        protected StringVariable loseReason;
        protected AwarenessManager awarenessManager = null;

        // used to update the entity's update every X frames
        [SerializeField] [FoldoutGroup("Debugging")] protected int pingFrequency;
        [SerializeField] [FoldoutGroup("Debugging")] protected int framesPassed;

        // used to know if the entity of type Camera is active
        [SerializeField] [FoldoutGroup("Debugging")] protected bool poweredOn;


        protected void Awake()
        {
            entityDataInterface = GetComponent<EntityDataInterface>();

            rangeOfVision = entityDataInterface.rangeOfVision;
            coneOfVision = entityDataInterface.coneOfVision;

            spottedPlayer = entityDataInterface.spottedPlayer;
            spottedDeadBody = entityDataInterface.spottedDeadBody;
            playerPeaking = entityDataInterface.spottedPlayer;
            alarmRaiserKilled = entityDataInterface.spottedDeadBody;
            baseState = entityDataInterface.baseState;

            awarenessManager = entityDataInterface.awarenessManagerObj.Value.GetComponent<AwarenessManager>();

            if (GetComponent<AgentDeath>() != null) entityType = EntityType.Guard;
            else entityType = EntityType.Camera;
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