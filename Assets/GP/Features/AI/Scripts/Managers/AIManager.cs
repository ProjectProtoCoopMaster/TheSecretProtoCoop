using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class AIManager : MonoBehaviour
    {
        public bool testAI;

        public static List<AgentManager> agents { get; protected set; } = new List<AgentManager>();

        void Start()
        {
            if (testAI) StartAllAgents();
        }

        public void StartAllAgents()
        {
            foreach (AgentManager agent in agents) agent.StartAgent();
        }
    } 
}
