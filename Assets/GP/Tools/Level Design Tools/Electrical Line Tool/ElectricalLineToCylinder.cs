using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Gameplay.Mobile;

namespace Tools
{
    public class ElectricalLineToCylinder : MonoBehaviour
    {
        [SerializeField] LineRenderer line;
        [SerializeField] GameObject prefabCylinder;
        private GameObject cylinder;

        [Button]
        public void ChangeLinesToCylinder()
        {


            line.GetComponent<ElectricalLineBehavior>().cylinders.Clear();

            for (int i = 0; i < line.positionCount - 1; i++)
            {
                if (line.GetPosition(i).x != line.GetPosition(i+1).x)
                {
                    cylinder = Instantiate(prefabCylinder, line.transform.position + line.GetPosition(i) + new Vector3((line.GetPosition(i + 1).x-line.GetPosition(i).x) * .5f, 0, 0), Quaternion.identity);
                    cylinder.transform.localScale = new Vector3(.35f, Mathf.Abs(line.GetPosition(i + 1).x - line.GetPosition(i).x) * .5f +.11f, .35f);
                    cylinder.transform.eulerAngles = new Vector3(0, 0, 90);
                }
                else
                {
                    cylinder = Instantiate(prefabCylinder, line.transform.position + line.GetPosition(i) + new Vector3(0, 0, line.GetPosition(i + 1).z - line.GetPosition(i).z) * .5f, Quaternion.identity);
                    cylinder.transform.localScale = new Vector3(.35f, Mathf.Abs(line.GetPosition(i + 1).z - line.GetPosition(i).z) * .5f +.11f , .35f);
                    cylinder.transform.eulerAngles = new Vector3(0, 90, 90);
                }

                cylinder.transform.parent = line.transform;
                line.GetComponent<ElectricalLineBehavior>().AddCylinder(cylinder);

            }

            line.GetComponent<LineRenderer>().enabled = false;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(line.GetComponent<ElectricalLineBehavior>());
            //for (int i = 0; i < UnityEditor.SceneManagement.EditorSceneManager.sceneCount; i++)
            //{
            //    UnityEngine.SceneManagement.Scene scene = UnityEditor.SceneManagement.EditorSceneManager.GetAllScenes()[i];
            //    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
            //}
            
#endif
        }
    }
}

