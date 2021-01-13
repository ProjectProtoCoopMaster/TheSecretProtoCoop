using System.Collections;
using System.Collections.Generic;
#if Unity_Editor
using UnityEditor;
#endif

using UnityEngine;
using Gameplay.Mobile;
using UnityEngine.PlayerLoop;
using UnityEditor;

namespace Tools.LevelDesign
{
    public class ElectricalDrawingView : MonoBehaviour
    {
        public bool isSelecting;
        public bool isMouse0Pressed;
        public int numberOfSelectionRect;
        public GameObject switcher;
        public GameObject line;
        public Canvas canvas;
        public GUIStyle importantStyle;
        public GUIStyle  mediumStyle;
        public GUIStyle  infoStyle;
        public List<Vector3> startPoint;
        public List<Vector3> endPoint;
        public List<Vector3> startTangent;
        public List<Vector3> endTangent;
        public bool isDrawingLine;
        public bool canDraw;
        public GameObject selectedGO;
        public GameObject firstSelectedGO;
        public GameObject secondSelectedGO;
        public string lastAction;
        public string currentAction;
        public string[] inputActions;
        

        public void CreateSwitcher()
        {
#if UNITY_EDITOR
            GameObject newSwitcher = PrefabUtility.InstantiatePrefab(switcher,canvas.transform) as GameObject;


            Selection.activeGameObject = newSwitcher;
#endif

            isDrawingLine = true;

        }


        
        public void CreateLine()
        {
            for (int i = 0; i < startPoint.Count; i++)
            {
#if UNITY_EDITOR
                GameObject newLine = PrefabUtility.InstantiatePrefab(line) as GameObject;


                newLine.GetComponent<ElectricalLinePlacement>().Initialize(firstSelectedGO, secondSelectedGO);
                newLine.GetComponent<LineRenderer>().SetPosition(0, startPoint[i]);
                newLine.GetComponent<LineRenderer>().SetPosition(1, startTangent[i]);
                newLine.GetComponent<LineRenderer>().SetPosition(2, endTangent[i]);
                newLine.GetComponent<LineRenderer>().SetPosition(3, endPoint[i]);
#endif
            }
            ClearLineLists();
            firstSelectedGO = null;
            secondSelectedGO = null;
            canDraw = false;

        }
        
        public void LinkPointsToGameObject()
        {
            startPoint[0] = firstSelectedGO.transform.position;
            endPoint[0] = new Vector3(secondSelectedGO.transform.position.x, firstSelectedGO.transform.position.y, secondSelectedGO.transform.position.z);
        }
        public void AddLine()
        {


            ClearLineLists();
            Vector3 secondPos = new Vector3(secondSelectedGO.transform.position.x, firstSelectedGO.transform.position.y, secondSelectedGO.transform.position.z);
            startPoint.Add(firstSelectedGO.transform.position);
            startTangent.Add(secondPos);
            endTangent.Add(secondPos);
            endPoint.Add(secondPos);


            

            isSelecting = false;
            canDraw = true;


        }

        public void ClearLineLists()
        {
            isDrawingLine = false;
            startPoint.Clear();
            endPoint.Clear();
            startTangent.Clear();
            endTangent.Clear();

        }

        public void ChangeTangentToALine()
        {
            int ID = startTangent.Count - 1;
            startTangent[ID] = endPoint[ID];
            endTangent[ID] = endPoint[ID];
        }

        public void ChangeTangentToUpLeft()
        {
            int ID = startTangent.Count - 1;
            startTangent[ID] = new Vector2(startPoint[ID].x, endPoint[ID].y); 
            endTangent[ID] = new Vector2(startPoint[ID].x, endPoint[ID].y);
        }
        public void ChangeTangentToDownLeft()
        {

            int ID = startTangent.Count - 1;
            startTangent[ID] = new Vector2(endPoint[ID].x, startPoint[ID].y); 
            endTangent[ID] = new Vector2(endPoint[ID].x, startPoint[ID].y);
        }

        public void MyReset()
        {
            isDrawingLine = false;
            canDraw = false;
            isSelecting = false;
            currentAction = "";
            firstSelectedGO = null;
            secondSelectedGO = null;
            selectedGO = null;

        }

    }
}

