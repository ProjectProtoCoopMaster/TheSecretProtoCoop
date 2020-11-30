using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public enum StateType { Patrol, Distraction, Search, Chase, None, Standby }

    public enum Usage { Start, Resume, End }

    public abstract class AgentManager : MonoBehaviour
    {
        public Dictionary<StateType, AgentBehavior> agentBehaviors = new Dictionary<StateType, AgentBehavior>();

        public StateType currentState { get; set; } = StateType.None;

        private StateType saveState = StateType.None;

        private bool resumeNext = false;

        public bool isDead { get; set; }

        void Awake()
        {
            InitializeAgent();
        }

        protected abstract void InitializeAgent();

        public void SwitchAgentState()
        {
            if (resumeNext)
            {
                SwitchAgentState(saveState, Usage.Resume, false);
            }
            else
            {
                SwitchAgentState(StateType.None, Usage.End, false); // Same as StopAgent();
            }
        }

        protected void SwitchAgentState(StateType _state, Usage _usage, bool resumeAfterwards)
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
