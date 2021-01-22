using Gameplay.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.VR
{
    public class AgentDeath : MonoBehaviour, IKillable
    {
        public AgentManager agent;

        [SerializeField] public UnityEvent deathEvent;

        public float thrust = 1.0f;

        [Button]
        public void Die(Vector3 force = default)
        {
            agent.ragdoll = Instantiate(agent.ragdollPrefab);
            agent.ragdoll.transform.parent = agent.transform;
            agent.ragdoll.transform.position = agent.transform.position;

            RagdollBehavior ragdollBehavior = agent.ragdoll.GetComponentInChildren<RagdollBehavior>();
            ragdollBehavior.ActivateRagdollWithForce(force, ForceMode.Impulse);

            deathEvent.Invoke();
        }

        public void Die() { }
    }
}
