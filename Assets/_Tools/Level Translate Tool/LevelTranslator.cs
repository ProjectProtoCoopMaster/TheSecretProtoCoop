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
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform switchablesParent;
        [SerializeField] private Camera cam;
        [SerializeField] private List<GameObject> prefabsVR;
        [SerializeField] private List<Image> prefabsMobile;
        public TextAsset json;
        [HideInInspector]
        [SerializeField] private LevelSaver.ListOfISwitchableElement elements;
        private Image newImage;

        public void TranslateLevelPosition()
        {
            elements = JsonUtility.FromJson<LevelSaver.ListOfISwitchableElement>(json.ToString());
            RectTransform parent = Object.Instantiate(switchablesParent, canvas.transform) as RectTransform;
            for (int i = 0; i < elements.list.Count; i++)
            {
                Vector3 position = elements.list[i].position;

                for (int j = 0; j < prefabsVR.Count; j++)
                {
                    if (elements.list[i].prefab.GetInstanceID() == prefabsVR[j].GetInstanceID()) 
                    {
#if UNITY_EDITOR
                        newImage = PrefabUtility.InstantiatePrefab(prefabsMobile[j] as Image, parent.transform) as Image;
#endif


                    }
                }
                
                newImage.rectTransform.anchoredPosition = position;
                
            }




        }
    }
}

