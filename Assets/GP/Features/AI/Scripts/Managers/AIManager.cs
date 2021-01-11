using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class AIManager : MonoBehaviour
    {
        public static List<AgentManager> agents { get; protected set; } = new List<AgentManager>();

        public void StartAllAgents()
        {
            foreach (AgentManager agent in agents) agent.StartAgent();
        }
    } 
}
