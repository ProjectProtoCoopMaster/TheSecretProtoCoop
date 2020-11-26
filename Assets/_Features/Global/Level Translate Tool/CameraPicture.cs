using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Tools.LevelDesign
{
    public class CameraPicture : MonoBehaviour
    {
        
        private Camera cam;
        public string pictureName;
        public string folderName;

        public void TakePicture()
        {
            cam = GetComponent<Camera>();

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;

            cam.Render();

            Texture2D tex2D = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
            tex2D.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
            tex2D.Apply();

            RenderTexture.active = currentRT;

            var Bytes = tex2D.EncodeToPNG();
            DestroyImmediate(tex2D);
            string path = "Assets/_Features/Global/Level Saver/Levels/" + folderName + "/";


            if (!File.Exists("Assets/_Features/Global/Level Saver/Levels/" + folderName + "/"))
            {
                Directory.CreateDirectory("Assets/_Features/Global/Level Saver/Levels/" + folderName + "/");
            }
            File.WriteAllBytes(path+pictureName+".png", Bytes);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }



    }
}

