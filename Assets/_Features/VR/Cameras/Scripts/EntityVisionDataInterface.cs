using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(OverwatchBehavior))]
    [RequireComponent(typeof(DetectionBehavior))]
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

        [SerializeField] protected AwarenessManager awarenessManager = null;

        protected Vector3 myPos, targetPos, myFinalPos;
        protected float sqrDistToTarget;
        protected bool poweredOn;

        [SerializeField] protected int pingFrequency;
        public int frames;

        private void Awake()
        {
            if (playerHead == null)
            {
                Debug.Log("Set Player Reference");
                playerHead = GameObject.Find("HeadCollider").transform;
            }
        }
    }
}