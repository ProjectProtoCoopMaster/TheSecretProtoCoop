using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SwitcherManager : MonoBehaviour
    {
        [ReadOnly] public SwitcherBehavior[] switchers;

        public void RaiseSwitch(float ID)
        {
            SearchSwitchersInScene();

            for (int i = 0; i < switchers.Length; i++)
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

        public void SearchSwitchersInScene() => switchers = FindObjectsOfType<SwitcherBehavior>();
    }
}
