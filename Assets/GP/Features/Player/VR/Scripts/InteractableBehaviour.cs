using Gameplay.AI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class InteractableBehaviour : MonoBehaviour
    {
        [Tooltip("Define which layers the sound will affect")] [SerializeField] LayerMask guardHearingLayermask;
        RaycastHit hitInfo;

        List<DistractionBehavior> agentsInScene = new List<DistractionBehavior>();
        [Tooltip("How much sound will be reduced when passing through a wall")] [SerializeField] [FoldoutGroup("Noise Values")] float wallDampenMultiplier = .5f;
        [Tooltip("How much sound will be reduced when passing through a wall")] [SerializeField] [FoldoutGroup("Noise Values")] float windowDampenMultiplier = .75f;
        [Tooltip("How far sound will travel")] [SerializeField] [FoldoutGroup("Noise Values")] float noiseRange;
        [Tooltip("How hard the object has to be thrown to produce maximum volume")] [SerializeField] [FoldoutGroup("Debugging")] float minimumVelocity;
        [Tooltip("The object's last range travelled (can appear reduced if it had to travel through walls)")] [SerializeField] [FoldoutGroup("Debugging")] float currNoiseRange;

        // accessed by the Grab Behaviour
        internal Rigidbody rigidBody;
        private AudioSource audioSource;

        public DistractionObjectGenerator generator;

        private bool isOnGround;

        public float timeBeforeRespawn;
        private float currentTime;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();

            agentsInScene.AddRange(FindObjectsOfType<DistractionBehavior>());
        }

        void Update()
        {
            if (isOnGround)
            {
                if (currentTime <= 0.0f)
                {
                    isOnGround = false;

                    generator.PoolObject(gameObject);
                    generator.SpawnObject();
                }

                currentTime -= Time.deltaTime;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > minimumVelocity)
            {
                MakeNoise();

                isOnGround = true;
                currentTime = timeBeforeRespawn;
            }
        }

        private void MakeNoise()
        {
            audioSource.Play();

            // check to see if the sound has to pass through walls to reach each enemy in the scene
            for (int i = 0; i < agentsInScene.Count; i++)
            {
                currNoiseRange = noiseRange;

                if (Physics.Linecast(transform.position, agentsInScene[i].transform.position, out hitInfo, guardHearingLayermask))
                {
                    if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Entity"))
                    {
                        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Window")) currNoiseRange *= wallDampenMultiplier;
                        else currNoiseRange *= wallDampenMultiplier;
                        Debug.Log("I had to hit " + hitInfo.collider.gameObject.name + " to reach " + agentsInScene[i].gameObject.name);
                    }
                }

                // if the agent is within range of the sound
                if ((agentsInScene[i].transform.position - transform.position).sqrMagnitude < currNoiseRange * currNoiseRange)
                {
                    Debug.Log(agentsInScene[i].gameObject.name + " heard that and is reacting");

                    GuardManager guard = agentsInScene[i].GetComponent<GuardManager>();

                    guard.DistractTo(transform.position);
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, currNoiseRange);
        }
    }
}