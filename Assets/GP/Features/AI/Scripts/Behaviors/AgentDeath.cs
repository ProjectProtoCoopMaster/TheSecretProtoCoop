using Sirenix.OdinInspector;
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

        public float thrust = 1.0f;

        [Button]
        public void Die(Vector3 force = default)
        {
            ragdollBehavior.ActivateRagdollWithForce(force, ForceMode.Impulse);            

            deathEvent.Invoke();
            
            enabled = false;
        }

        public void Die()
        {
           
        }
    } 
}
