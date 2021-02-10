using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class AIManager : MonoBehaviour
    {
        public bool testAI;

        [ReadOnly] public static List<AgentManager> agents = new List<AgentManager>();

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
