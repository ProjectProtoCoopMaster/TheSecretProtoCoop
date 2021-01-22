using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public class SwitcherManager : MonoBehaviour
    {
        public static SwitcherManager instance;

        public static List<SwitcherBehavior> switchers = new List<SwitcherBehavior>();

        
        void OnEnable() { if (instance == null) instance = this; }

        public void RaiseSwitch(float ID)
        {
            for (int i = 0; i < switchers.Count; i++)
            {
                if (switchers[i].ID == ID)
                {
                    switchers[i].TriggerSwitch();
                }
            }
        }

        public void StartAllSwitchers()
        {
            foreach (SwitcherBehavior switcher in switchers)
            {
                switcher.StartSwitcher();
            }
        }
    }
}

