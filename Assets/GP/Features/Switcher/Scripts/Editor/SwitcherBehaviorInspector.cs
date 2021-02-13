using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Gameplay.Mobile;

namespace Gameplay
{
    [CustomEditor(typeof(SwitcherBehavior))]
    public class SwitcherBehaviorInspector : Editor
    {
        SwitcherBehavior switcher;

        int numberOfChilds;

        private void OnEnable()
        {
            switcher = target as SwitcherBehavior;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(switcher.switchTimer == SwitcherBehavior.SwitchTimer.Fixed)
            {
                switcher.timer = EditorGUILayout.FloatField("Timer",switcher.timer);
            }

            if (switcher.hasJammer)
            {
                switcher.jammer = EditorGUILayout.ObjectField("Jammer", switcher.jammer, typeof(JammerBehavior)) as JammerBehavior;
            }

            switcher.SearchReferences();

            for (int i = 0; i < switcher.transform.childCount; i++)
            {
                if (switcher.transform.GetChild(i).gameObject.GetComponent<SwitcherBehavior>() != null)
                {
                    switcher.transform.GetChild(i).gameObject.GetComponent<SwitcherBehavior>().SearchReferences();
                    EditorUtility.SetDirty(switcher.transform.GetChild(i).gameObject.GetComponent<SwitcherBehavior>());
                }
            }

            Repaint();
            EditorUtility.SetDirty(switcher);
        }
    }
}
