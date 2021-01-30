using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay
{
    [CreateAssetMenu]
    public class CallableFunction : ScriptableObject
    {
        [SerializeField] private List<CallableFunctionListener> listeners = new List<CallableFunctionListener>();
        
        [Button]
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }
        
        public void Raise<T>(T parameter)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                if (listeners[i].ID > 0)
                {
                    for (int j = 0; j < listeners[i].ID; j++)
                    {

                        listeners[i].component[j].SendMessage(listeners[i].methodName[j], parameter);
                    }
                    //for (int i = listener.ID; i < listener.Response.GetPersistentEventCount(); i++)
                    //{

                    //    Type type = listener.Response.GetPersistentTarget(i).GetType();
                    //    type.GetMethod(listener.Response.GetPersistentMethodName(i)).Invoke(type, null);
                    //    //Component component;
                    //    //if(type == typeof(MonoBehaviour))
                    //    //{

                    //    //}
                    //    //Debug.Log(listener.TryGetComponent(type, out component));
                    //    //if (listener.TryGetComponent(type, out component))
                    //    //{

                    //    //}
                    //    //else
                    //    //{
                    //    //    type.GetMethod(listener.Response.GetPersistentMethodName(i)).Invoke(type, null);
                    //    //}

                    //}
                }
                else
                {
                    listeners[i].OnEventRaised();
                }



            }


        }

        public void RegisterListener(CallableFunctionListener listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(CallableFunctionListener listener)
        {
            listeners.Remove(listener);
        }

    }
}

