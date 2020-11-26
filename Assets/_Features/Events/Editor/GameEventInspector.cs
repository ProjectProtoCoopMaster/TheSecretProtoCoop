using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gameplay
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventInspector : Editor
    {
        GameEvent gameEvent;
        private void OnEnable()
        {
            gameEvent = target as GameEvent;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Raise"))
            {
                gameEvent.Raise();
            }
        }
    }
}

