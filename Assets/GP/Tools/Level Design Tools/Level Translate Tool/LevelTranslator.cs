using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;


namespace Tools.LevelDesign
{
    public class LevelTranslator : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private RectTransform switchablesParent;
        [SerializeField] private List<GameObject> prefabsVR;
        [SerializeField] private List<GameObject> prefabsMobile;
        public TextAsset json;
        [HideInInspector]
        [SerializeField] private LevelSaver.ListOfISwitchableElement elements;
        //private Image newImage;
        private GameObject newObject;
        public void TranslateLevelPosition()
        {
            elements = JsonUtility.FromJson<LevelSaver.ListOfISwitchableElement>(json.ToString());
            //RectTransform parent = Object.Instantiate(switchablesParent, canvas.transform) as RectTransform;
            for (int i = 0; i < elements.list.Count; i++)
            {
                Vector3 position = elements.list[i].position;
                Quaternion rotation = elements.list[i].rotation;
  
                for (int j = 0; j < prefabsVR.Count; j++)
                {
                    if (elements.list[i].prefab.GetInstanceID() == prefabsVR[j].GetInstanceID())
                    {
#if UNITY_EDITOR
                        newObject = PrefabUtility.InstantiatePrefab(prefabsMobile[j] as GameObject, parent) as GameObject;
#endif
                    }
                }

                newObject.transform.position = position;
                newObject.transform.rotation = rotation;

            }




        }
    }
}

