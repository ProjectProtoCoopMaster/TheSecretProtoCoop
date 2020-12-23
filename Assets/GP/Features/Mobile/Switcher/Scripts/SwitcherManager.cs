using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public class SwitcherManager : MonoBehaviour
    {
        [SerializeField] private SwitcherBehavior[] switchers;


        public void RaiseSwitch(float ID)
        {
            /*if (switchers.Length == 0 )*/ SearchSwitchersInScene();
            for (int i = 0; i < switchers.Length; i++)
            {
                if (switchers[i].ID == ID)
                {
                    switchers[i].TriggerSwitch();
                }
            }
        }

        public void SearchSwitchersInScene() => switchers = FindObjectsOfType<SwitcherBehavior>();

        
    }
}

