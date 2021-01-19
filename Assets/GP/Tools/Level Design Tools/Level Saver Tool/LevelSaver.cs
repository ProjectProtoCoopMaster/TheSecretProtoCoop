using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Gameplay;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tools.LevelDesign
{
    public class LevelSaver : MonoBehaviour
    {
        const string saveLocation = "Assets/GD/Level Design/Blockout/Levels/";

        //[SerializeField] private Camera cam;

        public GameObject[] parentSwitchers;

        public ListOfISwitchableElement elements;

        public string levelName;

        string fullPath;
        string JsonString;

        public void SaveJSON()
        {
            elements.list.Clear();

            for (int i = 0; i < parentSwitchers.Length; i++)
            {
                AddSwitcherChilds(parentSwitchers[i]);
            }

            fullPath = saveLocation + levelName+"/"+levelName+".json";

            if (!File.Exists(fullPath))
            {
                Directory.CreateDirectory(saveLocation + levelName + "/");
                File.Create(fullPath).Dispose();
            }
                
            JsonString = File.ReadAllText(fullPath);
            JsonString = JsonUtility.ToJson(elements,true);
            File.WriteAllText(fullPath, JsonString);

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        private void AddSwitcherChilds(GameObject switcher)
        {
            for (int j = 0; j < switcher.transform.childCount; j++)
            {
                if (switcher.transform.GetChild(j).GetComponent<ISwitchable>() != null)
                {
                    if (switcher.transform.GetChild(j).gameObject.GetComponent<SwitcherBehavior>())
                    {
                        AddSwitcherChilds(switcher.transform.GetChild(j).gameObject);
                    }
                    else
                    {
                        ISwitchableElements newElement = new ISwitchableElements();

#if UNITY_EDITOR
                        newElement.prefab = PrefabUtility.GetCorrespondingObjectFromSource(switcher.transform.GetChild(j).gameObject) as GameObject;
#endif

                        //newElement.position = cam.WorldToScreenPoint(switcher.transform.GetChild(j).position);
                        newElement.position = switcher.transform.GetChild(j).position;
                        newElement.rotation = switcher.transform.GetChild(j).rotation;
                        newElement.scale = switcher.transform.GetChild(j).transform.Find("Mesh").transform.localScale;
                        elements.list.Add(newElement);
                    }
                }
            }
        }

        [System.Serializable]
        public struct ISwitchableElements
        {
            public GameObject prefab;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;
        }

        [System.Serializable]
        public struct ListOfISwitchableElement
        {
            public List<ISwitchableElements> list;
        }
    }
}

