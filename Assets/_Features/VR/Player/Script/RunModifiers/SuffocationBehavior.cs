#if UNITY_STANDALONE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class SuffocationBehavior : AgentStateData
    {
        // suffocation behavior
        [SerializeField] protected float oxygenInSeconds;

        public void GE_DepleteOxygen()
        {
            StartCoroutine(DepleteOxygen());
        }

        IEnumerator DepleteOxygen()
        {
            oxygenInSeconds -= Time.deltaTime;
            yield return null;

            if (oxygenInSeconds <= 0) gameOver.Raise();
        }
    }
} 
#endif