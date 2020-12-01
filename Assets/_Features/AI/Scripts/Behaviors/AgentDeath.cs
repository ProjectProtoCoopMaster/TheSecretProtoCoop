using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    public class AgentDeath : MonoBehaviour, IKillable
    {
        public UnityEvent deathEvent;
        public RagdollBehavior ragdollBehavior;

        [Button]
        public void Die()
        {
            deathEvent.Invoke();
            ragdollBehavior.ActivateRagdoll();
            enabled = false;
        }
    } 
}
