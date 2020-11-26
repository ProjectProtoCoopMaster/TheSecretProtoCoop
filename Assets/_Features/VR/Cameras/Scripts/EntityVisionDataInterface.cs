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

        // overwatch variables (do not show in Scriptable)
        protected float pingFrequency = 2f; // frequency at which you check up on nearby entities
        protected List<GameObject> guards = new List<GameObject>(); // list of guards in the scene
        protected List<Vector3> deadGuards = new List<Vector3>(); // list of guards in the scene

        protected Vector3 myPos, targetPos, myFinalPos;
        protected float distToTarget;
        protected bool isActive;

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