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
        [Tooltip("This value dictates by how much sound will be reduced when passing through a wall")] [SerializeField] [FoldoutGroup("Noise Values")] float noiseDampenMultiplier = .5f;
        [Tooltip("This is the max range that sound should travel in an open room")] [SerializeField] [FoldoutGroup("Noise Values")] float maxRange;
        [Tooltip("This debug value defines how hard the object has to be thrown to produce maximum volume")] [SerializeField] [FoldoutGroup("Debugging")] float maxVelocityLimit;
        [Tooltip("This debug value returns the object's last range travelled (can appear reduced if it had to travel through walls)")] [SerializeField] [FoldoutGroup("Debugging")] float noiseRange;

        // accessed by the Grab Behaviour
        internal Rigidbody rigidBody;
        AudioSource audioSource;
        bool canPlaySound;

        private void Awake()
        {
            canPlaySound = false;
            
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            agentsInScene.AddRange(FindObjectsOfType<DistractionBehavior>());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > maxVelocityLimit) noiseRange = maxRange;
            else noiseRange = (float)CustomScaler.Scalef(collision.relativeVelocity.magnitude, 0, maxVelocityLimit, 0, maxRange);
            
            if(canPlaySound) MakeNoise(noiseRange);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                canPlaySound = true;
        }

        private void MakeNoise(float collisionForce)
        {
            //audioSource.volume = collisionForce / maxRange;
            audioSource.Play();

            canPlaySound = false;

            // check to see if the sound has to pass through walls to reach each enemy in the scene
            for (int i = 0; i < agentsInScene.Count; i++)
            {
                if (Physics.Linecast(transform.position, agentsInScene[i].transform.position, out hitInfo, guardHearingLayermask))
                {
                    if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Entity"))
                    {
                        collisionForce *= noiseDampenMultiplier;
                        Debug.Log("I had to hit " + hitInfo.collider.gameObject.name + " to reach " + agentsInScene[i].gameObject.name);
                    }
                }

                // if the agent is within range of the sound
                if ((agentsInScene[i].transform.position - transform.position).sqrMagnitude < collisionForce * collisionForce)
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
            Gizmos.DrawWireSphere(transform.position, noiseRange);
        }
    }
}