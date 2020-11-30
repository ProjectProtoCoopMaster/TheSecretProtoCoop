using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public enum GuardType { Patrol, Static }

    public class GuardManager : AgentManager
    {
        public GuardType guardType;

        [ShowIf("guardType", GuardType.Patrol)]
        public PatrolBehavior patrolBehavior;

        public DistractionBehavior distractionBehavior;

        protected override void InitializeAgent()
        {
            agentBehaviors = new Dictionary<StateType, AgentBehavior>();

            agentBehaviors.Add(StateType.Distraction, distractionBehavior); distractionBehavior.agent = this;
            if (guardType == GuardType.Patrol) { agentBehaviors.Add(StateType.Patrol, patrolBehavior); patrolBehavior.agent = this; }
        }

        void Start()
        {
            if (guardType == GuardType.Patrol)
            {
                SwitchAgentState(StateType.Patrol, Usage.Start, false);
            }
        }

        public void DistractTo(Vector3 destination)
        {
            distractionBehavior.SetDistraction(destination);

            SwitchAgentState(StateType.Distraction, Usage.Start, true);
        }
    } 
}
