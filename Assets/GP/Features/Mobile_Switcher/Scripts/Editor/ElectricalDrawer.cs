using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Tools.LevelDesign
{
    [CustomEditor(typeof(ElectricalDrawingView))]
    public class ElectricalDrawer : Editor
    {
        ElectricalDrawingView t;
        Rect selectionRect;
        Vector2 mousePos;
        Event current;

        private void OnEnable()
        {
            t = target as ElectricalDrawingView;
        }
        private void OnSceneGUI()
        {
            current = Event.current;

            t.selectedGO = Selection.activeGameObject;




            if (current.type == EventType.MouseDown)
            {
                if (current.button == 1)
                {
                    t.isSelecting = true;
                    OpenSelectionPanel();
                }
                else if (current.button == 0)
                {
                    if (!selectionRect.Contains(HandleUtility.GUIPointToWorldRay(current.mousePosition).origin))
                    {
                        t.isSelecting = false;
                    }

                    t.isMouse0Pressed = true;

                }

            }
            else if (current.type == EventType.MouseUp )
            {
                if (current.button == 0)
                {
                    t.isMouse0Pressed = false;
                }

            
            }

            if (t.isSelecting)
            {
                DrawSelectionRects();
                HandleUtility.AddDefaultControl(0);
            }





            
            if (t.canDraw) { DrawLines(); t.LinkPointsToGameObject(); t.currentAction = "Creating A Line"; }
            if (t.isDrawingLine) { if (t.firstSelectedGO == null) t.currentAction = "Selecting First GameObject : " + t.selectedGO;
                                   else t.currentAction = "Selecting Second GameObject : " + t.selectedGO; }

            if (current.type == EventType.KeyDown)
            {
                if (current.keyCode == KeyCode.A)
                {
                    if (t.canDraw)
                    {
                        MyUndo("Create Line");
                        t.currentAction = "";
                        t.CreateLine();

                        current.Use();
                    }

                }
                else if (current.keyCode == KeyCode.E)
                {
                    if (t.canDraw)
                    {
                        MyUndo("Change Tangent To A Line");
                        t.ChangeTangentToALine();
                        current.Use();
                    }

                }
                else if (current.keyCode == KeyCode.R)
                {
                    if (t.canDraw)
                    {
                        MyUndo("Change Tangent To A Right Angle");
                        t.ChangeTangentToUpLeft();
                        current.Use();
                    }
                }
                else if (current.keyCode == KeyCode.T)
                {
                    if (t.canDraw)
                    {
                        MyUndo("Change Tangent To A Right Angle");
                        t.ChangeTangentToDownLeft();
                        current.Use();
                    }

                }
                else if (current.keyCode == KeyCode.Escape)
                {
                    MyUndo("Reset");
                    t.MyReset();
                    current.Use();
                }
                else if (current.keyCode == KeyCode.Space)
                {
                    if (t.isDrawingLine)
                    {
                        if (t.firstSelectedGO == null)
                        {
                            MyUndo("Select First Object");
                            t.firstSelectedGO = t.selectedGO;

                            current.Use();
                        }
                        else if (t.secondSelectedGO == null)
                        {
                            MyUndo("Select Second Object");
                            t.secondSelectedGO = t.selectedGO;
                            
                            
                            t.AddLine();
                            current.Use();
                        }
                    }
                }
            }



            DrawLabels();
            SceneView.currentDrawingSceneView.Repaint();
            HandleUtility.Repaint();

        }

        private void DrawLabels()
        {
            Rect screenRect = SceneView.currentDrawingSceneView.camera.pixelRect;
            Vector3 labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10,screenRect.height - 10));
            labelPos = new Vector3(labelPos.x, labelPos.y, 0);

            if(t.currentAction != "")
            {
                Handles.Label(labelPos, "Current Action : " + t.currentAction, t.importantStyle);
                labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 35));
                labelPos = new Vector3(labelPos.x, labelPos.y, 0);
            }

            if (t.isDrawingLine)
            {

                for (int i = 0; i <3; i++)
                {

                    switch (i)
                    {
                        case 0:
                            Handles.Label(labelPos, "Space : Register GameObject After you've Selected It", t.infoStyle);
                            break;
                        case 1:
                            labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 60));
                            labelPos = new Vector3(labelPos.x, labelPos.y, 0);
                            Handles.Label(labelPos, "Left Click On GameObject :  Select It", t.infoStyle);
                            break;
                        case 2:
                            labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 85));
                            labelPos = new Vector3(labelPos.x, labelPos.y, 0);
                            Handles.Label(labelPos, "Escape : Reset", t.infoStyle);
                            break;

                    }

                }

                labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 110));
                labelPos = new Vector3(labelPos.x, labelPos.y, 0);

            }
            else if (t.canDraw)
            {
                for (int i = 0; i < 5; i++)
                {

                    switch (i)
                    {
                        case 0:
                            Handles.Label(labelPos, "A : Create A Line", t.infoStyle);
                            break;
                        case 1:
                            labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 60));
                            labelPos = new Vector3(labelPos.x, labelPos.y, 0);
                            Handles.Label(labelPos, "E : Make Line Straight", t.infoStyle);
                            break;
                        case 2:
                            labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 85));
                            labelPos = new Vector3(labelPos.x, labelPos.y, 0);
                            Handles.Label(labelPos, "R : Create A Right Angle", t.infoStyle);
                            break;
                        case 3:
                            labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 110));
                            labelPos = new Vector3(labelPos.x, labelPos.y, 0);
                            Handles.Label(labelPos, "T : Create A Right Angle", t.infoStyle);
                            break;
                        case 4:
                            labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 135));
                            labelPos = new Vector3(labelPos.x, labelPos.y, 0);
                            Handles.Label(labelPos, "Escape : Reset", t.infoStyle);
                            break;
                    }
                    labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 160));
                    labelPos = new Vector3(labelPos.x, labelPos.y, 0);

                }
            }

            else
            {

                for (int i = 0; i < 1; i++)
                {

                    switch (i)
                    {
                        case 0:
                            Handles.Label(labelPos, "Right Click : Open Selecting Panel", t.infoStyle);
                            break;

                    }

                }
                labelPos = SceneView.lastActiveSceneView.camera.ScreenToWorldPoint(new Vector2(10, screenRect.height - 35));
                labelPos = new Vector3(labelPos.x, labelPos.y, 0);

            }


            Handles.Label(labelPos, "Last Action : " + t.lastAction,t.mediumStyle);

            

        }
        private void OpenSelectionPanel()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);
            mousePos = ray.origin;
        }

        private void DrawSelectionRects()
        {

            if (t.numberOfSelectionRect == 1)
                selectionRect = new Rect(mousePos.x, mousePos.y, 4, (1.5f * t.numberOfSelectionRect));
            else selectionRect = new Rect(mousePos.x, mousePos.y, 4, (1.5f * t.numberOfSelectionRect - (t.numberOfSelectionRect * .4f)));


            
            Handles.DrawSolidRectangleWithOutline(selectionRect, Color.cyan, Color.black);
            for (int i = 0; i < t.numberOfSelectionRect; i++)
            {

                Rect genericRect = new Rect(selectionRect.x + selectionRect.width - 3.5f , selectionRect.y + selectionRect.height -1.1f, 3, .8f);
                Rect switcherRect = new Rect(genericRect.x, genericRect.y- (i * (genericRect.height *1.1f) ), genericRect.width, genericRect.height);

                if (switcherRect.Contains(HandleUtility.GUIPointToWorldRay(current.mousePosition).origin))
                {
                   
                    if (t.isMouse0Pressed)
                    {
                        Handles.DrawSolidRectangleWithOutline(switcherRect, Color.red, Color.black);
                        if (current.type == EventType.MouseDown)
                        {
                            switch (i)
                            {
                                case 0:
                                    MyUndo("Create Switcher");
                                    t.CreateSwitcher();
                                    current.Use();
                                    break;
                                case 1:
                                    MyUndo("Start Drawing Line");
                                    t.isDrawingLine = true;
                                    current.Use();
                                    break;
                            }

                        }
                            

                    }
                    else
                    {
                        Handles.DrawSolidRectangleWithOutline(switcherRect, Color.cyan, Color.black);
                    }

                }
                else
                {
                    Handles.DrawSolidRectangleWithOutline(switcherRect, Color.cyan, Color.black);
                }

            }


        }

        private void DrawLines()
        {


            EditorGUI.BeginChangeCheck();
            int ID = t.startPoint.Count - 1;

            
            t.startPoint[ID] = Handles.PositionHandle(t.startPoint[ID], Quaternion.identity);
            Handles.DrawSolidRectangleWithOutline(new Rect(t.startPoint[ID].x, t.startPoint[ID].y, .1f, .1f), Color.red, Color.black);
            
            t.endPoint[ID] = Handles.PositionHandle(t.endPoint[ID], Quaternion.identity);
            Handles.DrawSolidRectangleWithOutline(new Rect(t.endPoint[ID].x, t.endPoint[ID].y, .1f, .1f), Color.magenta, Color.black);
            t.startTangent[ID] = Handles.PositionHandle(t.startTangent[ID], Quaternion.identity);
            Handles.DrawSolidRectangleWithOutline(new Rect(t.startTangent[ID].x, t.startTangent[ID].y, .1f, .1f), Color.green, Color.black);
            t.endTangent[ID] = Handles.PositionHandle(t.endTangent[ID], Quaternion.identity);
            Handles.DrawSolidRectangleWithOutline(new Rect(t.endTangent[ID].x, t.endTangent[ID].y, .1f, .1f), Color.cyan, Color.black);


            for (int i = 0; i < t.startPoint.Count; i++)
            {
                Handles.DrawBezier(t.startPoint[i], t.endPoint[i], t.startTangent[i], t.endTangent[i], Color.red, null, 5f);
            }

            if (EditorGUI.EndChangeCheck())
            {
                MyUndo("Handle Changes");
            }

        }

        private void MyUndo(string text)
        {
            Undo.RecordObject(t, text);
            t.lastAction = text;
        }

    }
}

