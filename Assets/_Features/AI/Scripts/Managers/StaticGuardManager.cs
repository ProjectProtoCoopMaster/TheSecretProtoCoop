using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class StaticGuardManager : AgentManager
    {
        public DistractionBehavior distractionBehavior;

        protected override void InitializeAgent()
        {
            agentBehaviors = new Dictionary<StateType, AgentBehavior>()
            {
                { StateType.Distraction, distractionBehavior }
            };
        }
    } 
}
