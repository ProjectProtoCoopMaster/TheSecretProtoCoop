using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class JammerManager : MonoBehaviour
    {
        public static JammerManager instance;

        public static List<JammerBehavior> jammers = new List<JammerBehavior>();

        void OnEnable() { if (instance == null) instance = this; }

        public void DestroyJammer(int ID)
        {
            for (int i = 0; i < jammers.Count; i++)
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
    }

}
