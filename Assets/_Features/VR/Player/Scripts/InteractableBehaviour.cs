using Gameplay.AI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]

    public class InteractableBehaviour : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        RaycastHit[] hitInfo;

        [SerializeField] float maxRange;
        float noiseRange;

        // accessed by the Grab Behaviour
        internal Rigidbody rigidBody;
        AudioSource audioSource;
        bool canPlaySound;

        [SerializeField] List<DistractionBehavior> agentsInScene = new List<DistractionBehavior>();

        private void Awake()
        {
            if (gameObject.tag != "Interactable") gameObject.tag = "Interactable";
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();

            agentsInScene.AddRange(FindObjectsOfType<DistractionBehavior>());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment") && canPlaySound)
                MakeNoise(noiseRange = collision.relativeVelocity.magnitude);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                canPlaySound = true;
        }

        private void MakeNoise(float collisionForce)
        {
            Debug.Log("I hit the target at a force of " + collisionForce);
            audioSource.volume = collisionForce / maxRange;
            audioSource.Play();

            canPlaySound = false;
            
            // check to see if the sound has to pass through walls to reach each enemy in the scene
            for (int i = 0; i < agentsInScene.Count; i++)
            {
                hitInfo = Physics.RaycastAll(transform.position, agentsInScene[i].transform.position, 500f, layerMask);

                if(hitInfo.Length > 0)
                {
                    for (int j = 0; j < hitInfo.Length; j++)
                    {
                        // if the sound has to pass through a wall
                        if (hitInfo[j].collider.CompareTag("Wall") || hitInfo[j].collider.CompareTag("Window"))
                        {
                            noiseRange *= .5F;
                            Debug.Log("I had to hit " + hitInfo[j].collider.gameObject.name + " to reach " + agentsInScene[i].gameObject.name);
                            Debug.Log("noiseRange is now " + noiseRange);
                        }
                    }
                }

                // if the agent is within range of the sound
                if ((agentsInScene[i].transform.position - transform.position).sqrMagnitude < noiseRange * noiseRange)
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