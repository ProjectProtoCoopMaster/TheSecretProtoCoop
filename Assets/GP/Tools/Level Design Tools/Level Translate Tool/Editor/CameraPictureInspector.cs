using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Tools.LevelDesign
{
    [CustomEditor(typeof(CameraPicture))]
    public class CameraPictureInspector : Editor
    {
        CameraPicture t;
        private void OnEnable()
        {
            t = target as CameraPicture;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (t.pictureName != "")
            {
                if (File.Exists(Application.dataPath + "/_Features/Global/Level Translate Tool/Pictures/" + t.pictureName + ".png"))
                {
                    EditorGUILayout.HelpBox("You are ready to overwrite a picture with the same name... are you sure ?", MessageType.Warning);
                }
                if (GUILayout.Button("Take Picture"))
                {
                    

                    t.TakePicture();
                }
            }
            
        }

    }
}

