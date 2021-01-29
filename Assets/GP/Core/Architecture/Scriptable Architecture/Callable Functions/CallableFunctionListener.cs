using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public class CallableFunctionListener : MonoBehaviour
    {
        public CallableFunction Function;
        [InfoBox("Number of target in response in which you want to pass the callable function parameter")]
        public int ID;

        public UnityEvent Response;
        public Type[] type;
        [HideInInspector] public string[] methodName;
        [HideInInspector] public Component[] component;

        public void OnEventRaised()
        {
            Response.Invoke();
        }

        private void OnEnable()
        {
            Function.RegisterListener(this);
            type = new Type[ID];
            methodName = new string[ID];
            component = new Component[ID];

            for (int i = 0; i < ID; i++)
            {
                type[i] = Response.GetPersistentTarget(i).GetType();
                methodName[i] = Response.GetPersistentMethodName(i);
                component[i] = GetComponent(type[i]);
            }
        }

        private void OnDisable()
        {
            Function.UnRegisterListener(this);
        }
    }
}
