using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class EntityVisionDataInterface : MonoBehaviour
    {
        [SerializeField] [HideInInspector] public EntityVisionScriptable entityVisionData;

        [SerializeField] public float rangeOfVision;
        [SerializeField] public float coneOfVision;
        [SerializeField] [HideInInspector] public Transform playerHead;

        [SerializeField] public LayerMask detectionMask;
        [SerializeField] [HideInInspector] protected RaycastHit hitInfo;

        [SerializeField] protected GameEvent raiseAlarm;
        [SerializeField] [HideInInspector] protected Vector3 targetDir;

        protected AwarenessManager awarenessManager = null;

        protected Vector3 myPos, targetPos, myFinalPos;
        protected float sqrDistToTarget;

        [SerializeField] protected bool poweredOn;

        [SerializeField] protected int pingFrequency;
        public int frames;

        [SerializeField] EntityType entityType;

        private void Awake()
        {
            playerHead = GameObject.Find("HeadCollider").transform;
            awarenessManager = FindObjectOfType<AwarenessManager>();
        }
    }
}