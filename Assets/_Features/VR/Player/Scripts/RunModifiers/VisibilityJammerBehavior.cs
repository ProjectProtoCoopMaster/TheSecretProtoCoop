#if UNITY_STANDALONE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class VisibilityJammerBehavior : AgentStateData
    {
        // visibility jammer behavior
        [SerializeField] protected float anotherVariable;

        public void GE_VisibilityJammed()
        {

        }
    }
} 
#endif