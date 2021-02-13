using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Gameplay
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        [SerializeField] private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            Debug.LogWarning(name);

            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }

}
