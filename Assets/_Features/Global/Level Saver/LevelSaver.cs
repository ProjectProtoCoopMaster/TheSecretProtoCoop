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
        [SerializeField] private Camera cam;
        public GameObject[] parentSwitchers;
        public ListOfISwitchableElement elements;
        public string levelName;

        
        string path, JsonString;
        public void SaveJSON()
        {
            elements.list.Clear();
            for (int i = 0; i < parentSwitchers.Length; i++)
            {
                AddSwitcherChilds(parentSwitchers[i]);
            }

            
            path = "Assets/_Features/Global/Level Saver/Levels/"+ levelName+"/"+levelName+".json";
            if (!File.Exists(path)) 
            {
                Directory.CreateDirectory("Assets/_Features/Global/Level Saver/Levels/" + levelName + "/");
                File.Create(path).Dispose();

            }
                
            JsonString = File.ReadAllText(path);
            JsonString = JsonUtility.ToJson(elements,true);
            File.WriteAllText(path, JsonString);
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

                        newElement.position = cam.WorldToScreenPoint(switcher.transform.GetChild(j).position);
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
        }
        [System.Serializable]
        public struct ListOfISwitchableElement
        {
            public List<ISwitchableElements> list;
        }
    }

}

