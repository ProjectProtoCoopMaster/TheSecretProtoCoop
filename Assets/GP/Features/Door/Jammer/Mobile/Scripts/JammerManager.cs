using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class JammerManager : MonoBehaviour
    {
        [SerializeField] private JammerBehavior[] jammers;


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

        public void SearchJammersInScene() => jammers = FindObjectsOfType<JammerBehavior>();
    }

}
