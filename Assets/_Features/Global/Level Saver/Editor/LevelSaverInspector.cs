using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Tools.LevelDesign
{

    [CustomEditor(typeof(LevelSaver))]
    public class LevelSaverInspector : Editor
    {
        private LevelSaver t;
        private void OnEnable()
        {
            t = target as LevelSaver;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            

            if (GUILayout.Button("Save JSON"))
            {
                t.SaveJSON();
            }
        }
    }
}

