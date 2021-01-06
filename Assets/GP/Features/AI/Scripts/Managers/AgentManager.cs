using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public enum StateType { Patrol, Distraction, Search, Chase, None, Standby }

    public enum Usage { Start, Resume, End }

    public abstract class AgentManager : MonoBehaviour
    {
        protected Dictionary<StateType, AgentBehavior> agentBehaviors = new Dictionary<StateType, AgentBehavior>();

        public StateType currentState { get; protected set; } = StateType.None;

        protected StateType saveState = StateType.None;

        protected bool resumeNext = false;

        public bool isDead { get; protected set; }

        void OnEnable() => AIManager.agents.Add(this);

        void OnDisable() => AIManager.agents.Remove(this);

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

        protected void SwitchAgentState(Usage _usage, StateType _state = StateType.None, bool resumeAfterwards = false)
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

        protected void StopAgent()
        {
            foreach (AgentBehavior agentBehavior in agentBehaviors.Values)
            {
                agentBehavior.Stop();
            }
            currentState = StateType.None;
        }

        public void KillAgent()
        {
            StopAgent();

            isDead = true;
        }
    }
}
