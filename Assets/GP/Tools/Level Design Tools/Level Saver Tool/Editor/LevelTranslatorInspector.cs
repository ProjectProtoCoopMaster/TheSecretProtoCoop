using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.LevelDesign
{
    [CustomEditor(typeof(LevelTranslator))]
    public class LevelTranslatorInspector : Editor
    {
        LevelTranslator t;

        private void OnEnable()
        {
            t = target as LevelTranslator;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Translate Level"))
            {
                t.TranslateLevelPosition();
            }
        }
    }
}

