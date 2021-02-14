using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class AIManager : MonoBehaviour
    {
        public bool testAI;

        [ShowInInspector, ReadOnly]
        public static List<AgentManager> agents = new List<AgentManager>();
        public List<AgentManager> Agents { get => agents; }

        // Unused
        public void FindAllAgents()
        {
            agents.Clear();
            AgentManager[] agentsFound = FindObjectsOfType<AgentManager>();
            foreach (AgentManager agent in agentsFound) agents.Add(agent);
        }

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
