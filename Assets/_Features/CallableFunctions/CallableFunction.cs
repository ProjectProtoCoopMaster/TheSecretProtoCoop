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
        [SerializeField] private CallableFunctionListener listener = new CallableFunctionListener();


        [Button]
        public void Raise()
        {
            listener.OnEventRaised();
        }
        
        public void Raise<T>(T parameter)
        {
            for (int i = 0; i < listener.ID; i++)
            {
                
                listener.component[i].SendMessage(listener.methodName[i], parameter);
                Debug.Log(listener.component[i]);
            }

        }

        public void RegisterListener(CallableFunctionListener otherListener)
        {
            listener = otherListener;
        }

        public void UnRegisterListener()
        {
            listener = null;
        }

    }
}

