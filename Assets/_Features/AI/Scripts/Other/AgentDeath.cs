using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    public class AgentDeath : MonoBehaviour
    {
        public UnityEvent deathEvent;
        public RagdollBehavior ragdollBehavior;

        public void Die()
        {
            gameObject.name = "DEAD";
            ragdollBehavior.ActivateRagdoll();

            deathEvent.Invoke();
        }
    } 
}
