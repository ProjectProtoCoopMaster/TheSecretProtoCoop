using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    public class AgentDeath : MonoBehaviour, IKillable
    {
        [SerializeField] public UnityEvent deathEvent;

        [SerializeField] private RagdollBehavior ragdollBehavior;

        public void Die()
        {
            ragdollBehavior.ActivateRagdoll();

            deathEvent.Invoke();
            
            enabled = false;
        }
    } 
}
