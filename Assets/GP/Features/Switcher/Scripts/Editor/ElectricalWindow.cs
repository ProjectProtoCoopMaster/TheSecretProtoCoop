using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Gameplay;


public class ElectricalWindow : EditorWindow
{
    GameObject switcher;
    GameObject link;
    Canvas canvas;

    [MenuItem("Window/Electrical Window")]

    public static void Init()
    {
        ElectricalWindow window = EditorWindow.GetWindow(typeof(ElectricalWindow)) as ElectricalWindow;
        window.Show();
    }

    private void OnGUI()
    {
        switcher = EditorGUILayout.ObjectField(switcher, typeof(GameObject)) as GameObject;
        link = EditorGUILayout.ObjectField(link, typeof(GameObject)) as GameObject;
        canvas = EditorGUILayout.ObjectField(canvas, typeof(Canvas)) as Canvas;

        if (GUILayout.Button("Create Links Between Switcher and Node"))
        {
            CreateLinks();
        }
    }

    private void CreateLinks()
    {
        for (int i = 0; i < switcher.transform.childCount; i++)
        {
            if(switcher.transform.GetChild(i).GetComponent<ISwitchable>() != null)
            {
                GameObject newLink = Instantiate(link);
                newLink.transform.parent = canvas.transform;
                newLink.transform.localScale = Vector3.one;

                RectTransform newLinkRect = newLink.GetComponent<RectTransform>();
                RectTransform childRect = switcher.transform.GetChild(i).GetComponent<RectTransform>();

                Vector2 pos = (childRect.anchoredPosition + (childRect.sizeDelta / 2)) - (switcher.GetComponent<RectTransform>().anchoredPosition + (switcher.GetComponent<RectTransform>().sizeDelta / 2));
                Debug.Log(childRect.sizeDelta);
                newLink.GetComponent<RectTransform>().anchoredPosition = pos *.5f;
            }
        }
    }
}
