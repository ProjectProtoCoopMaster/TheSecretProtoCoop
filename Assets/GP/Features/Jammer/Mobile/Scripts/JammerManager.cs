using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class JammerManager : MonoBehaviour
    {
        [ReadOnly] public JammerBehavior[] jammers;

        public void DestroyJammer(int ID)
        {
            SearchJammersInScene();

            for (int i = 0; i < jammers.Length; i++)
            {
                if (jammers[i].ID == ID)
                {
                    jammers[i].SetState(false);
                }
            }
        }

        public void StartAllJammers()
        {
            foreach (JammerBehavior jammer in jammers)
            {
                jammer.SetState(true);
            }
        }

        public void SearchJammersInScene() => jammers = FindObjectsOfType<JammerBehavior>();
    }
}
