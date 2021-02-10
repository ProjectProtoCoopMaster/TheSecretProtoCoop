using Gameplay.AI;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundMaker : MonoBehaviour
    {
        [Tooltip("Define which layers the sound will affect")] [SerializeField] protected LayerMask guardHearingLayermask;
        protected RaycastHit hitInfo;

        List<AgentManager> agents { get => AIManager.agents; }

        [Tooltip("How much sound will be reduced when passing through a wall")] [SerializeField] [FoldoutGroup("Noise Values")] protected float wallDampenMultiplier = .5f;
        [Tooltip("How much sound will be reduced when passing through a wall")] [SerializeField] [FoldoutGroup("Noise Values")] protected float windowDampenMultiplier = .75f;
        [Tooltip("How far sound will travel")] [SerializeField] [FoldoutGroup("Noise Values")] protected float noiseRange = 7f;
        [Tooltip("How hard the object has to be thrown to produce maximum volume")] [SerializeField] [FoldoutGroup("Debugging")] protected float minimumVelocity = 7f;
        [Tooltip("The object's last range travelled (can appear reduced if it had to travel through walls)")] [SerializeField] [FoldoutGroup("Debugging")] protected float currNoiseRange;

        protected AudioSource audioSource;

        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected void Update() { }

        protected void MakeNoise()
        {
            audioSource.Play();

            // check to see if the sound has to pass through walls to reach each enemy in the scene
            for (int i = 0; i < agents.Count; i++)
            {
                currNoiseRange = noiseRange;

                if (Physics.Linecast(transform.position, agents[i].transform.position, out hitInfo, guardHearingLayermask))
                {
                    if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Entity"))
                    {
                        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Window")) currNoiseRange *= wallDampenMultiplier;
                        else currNoiseRange *= wallDampenMultiplier;
                        //Debug.Log("I had to hit " + hitInfo.collider.gameObject.name + " to reach " + agentsInScene[i].gameObject.name);
                    }
                }

                // if the agent is within range of the sound
                if ((agents[i].transform.position - transform.position).sqrMagnitude < currNoiseRange * currNoiseRange)
                {
                    GuardManager guard = agents[i].GetComponent<GuardManager>();
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