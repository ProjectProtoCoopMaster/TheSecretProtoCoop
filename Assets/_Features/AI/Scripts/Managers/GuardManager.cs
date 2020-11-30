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

            agentBehaviors.Add(StateType.Distraction, distractionBehavior);
            if (guardType == GuardType.Patrol) agentBehaviors.Add(StateType.Patrol, patrolBehavior);
        }

        void Start()
        {
            if (guardType == GuardType.Patrol)
            {
                currentState = StateType.Patrol;

                agentBehaviors[currentState].Begin();
            }
        }
    } 
}
