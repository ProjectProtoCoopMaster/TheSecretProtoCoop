using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class EntityVisionData : MonoBehaviour
    {
        // need to be identical across both Detection and Overwatch
        [SerializeField] public float rangeOfVision;
        [SerializeField] public float coneOfVision;

        // cached cone of vision/detection values
        protected Vector3 targetDir;
        protected Vector3 myPos, targetPos, myFinalPos;
        protected float sqrDistToTarget;

        // usued for the Awareness Manager to determine who detected
        internal EntityType entityType;

        // used to update the entity's update every X frames
        [SerializeField] [FoldoutGroup("Debugging")] protected int pingFrequency;
        [SerializeField] [FoldoutGroup("Debugging")] protected int framesPassed;

        // used to know if the entity of type Camera is active
        [SerializeField] [FoldoutGroup("Debugging")] protected bool poweredOn;

        // used for raising the alarm
        protected AwarenessManager awarenessManager = null;

        protected void Awake()
        {
            if (GetComponent<AgentDeath>() != null) entityType = EntityType.Guard;
            else entityType = EntityType.Camera;

            awarenessManager = FindObjectOfType<AwarenessManager>();
        }

        protected void RaiseAlarm()
        {

        }
    }
}