using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class PatrolGuardManager : AgentManager
    {
        public PatrolBehavior patrolBehavior;
        public DistractionBehavior distractionBehavior;

        protected override void InitializeAgent()
        {
            agentBehaviors = new Dictionary<StateType, AgentBehavior>()
            {
                { StateType.Patrol, patrolBehavior },
                { StateType.Distraction, distractionBehavior }
            };
        }

        void Start()
        {
            currentState = StateType.Patrol;

            agentBehaviors[currentState].Begin();
        }
    } 
}
