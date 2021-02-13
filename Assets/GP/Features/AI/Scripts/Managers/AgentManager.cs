using Gameplay.VR;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Gameplay.AI
{
    public enum StateType { Patrol, Distraction, Search, Chase, None, Standby }

    public enum Usage { Start, Resume, End }

    public abstract class AgentManager : MonoBehaviour, IKillable
    {
        protected Dictionary<StateType, AgentBehavior> agentBehaviors = new Dictionary<StateType, AgentBehavior>();

        public StateType currentState { get; protected set; } = StateType.None;

        protected StateType saveState = StateType.None;

        protected bool resumeNext = false;

        public bool isDead { get; set; }

        [Title("Agent")]

        public NavMeshAgent navMeshAgent;

        public Transform agentRig;
        public GameObject ragdollPrefab; public GameObject ragdoll { get; set; }

        [Title("Death")]

        public UnityEvent deathEvent;
        public float thrust = 1.0f;

        private void OnEnable() => AIManager.agents.Add(this);
        private void OnDisable() => AIManager.agents.Remove(this);

        void Awake()
        {
            InitializeAgent();
        }

        protected abstract void InitializeAgent();

        public abstract void StartAgent();

        public void SwitchAgentState()
        {
            if (resumeNext)
            {
                SwitchAgentState(Usage.Resume, saveState);
            }
            else
            {
                SwitchAgentState(Usage.End); // Same as StopAgent();
            }
        }

        public void SwitchAgentState(Usage _usage, StateType _state = StateType.None, bool resumeAfterwards = false)
        {
            if (isDead) return;

            if (resumeAfterwards && currentState != StateType.None)
            {
                saveState = currentState;

                resumeNext = true;
            }

            StopAgent();
            if (_usage == Usage.End) return;

            else if (_usage == Usage.Resume) agentBehaviors[_state].Resume();

            else if (_usage == Usage.Start) agentBehaviors[_state].Begin();

            currentState = _state;
        }

        public void StopAgent()
        {
            foreach (AgentBehavior agentBehavior in agentBehaviors.Values)
            {
                agentBehavior.Stop();
            }
            currentState = StateType.None;
        }

        [Button]
        public void Die(Vector3 force = default)
        {
            if (!isDead)
            {
                Debug.Log("Kill Agent");

                StopAgent();

                agentRig.gameObject.SetActive(false);

                ragdoll = Instantiate(ragdollPrefab);
                ragdoll.transform.parent = transform;
                ragdoll.transform.position = transform.position;
                RagdollBehavior ragdollBehavior = ragdoll.GetComponentInChildren<RagdollBehavior>();
                ragdollBehavior.ActivateRagdollWithForce(force, ForceMode.Impulse);

                deathEvent.Invoke();

                isDead = true;
            }
        }

        public void Revive(Vector3 pos, Quaternion rot)
        {
            agentRig.gameObject.SetActive(true);

            Debug.Log(pos);
            transform.position = pos;
            transform.rotation = rot;

            if (ragdoll != null)
            {
                Debug.Log("Previous Ragdoll Destroyed");
                Destroy(ragdoll);
            }

            isDead = false;
        }
    }
}
