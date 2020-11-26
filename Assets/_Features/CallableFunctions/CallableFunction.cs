using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu]
    public class CallableFunction : ScriptableObject
    {
        [SerializeField] private CallableFunctionListener listener = new CallableFunctionListener();
        public void Raise()
        {
            listener.OnEventRaised();
        }
        public void Raise<T>(T parameter) => listener.component.SendMessage(listener.methodName, parameter);

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

