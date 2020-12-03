#if UNITY_STANDALONE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class AgentStateData : MonoBehaviour
    {
        [SerializeField] protected GameEvent gameOver;
    }
}
#endif