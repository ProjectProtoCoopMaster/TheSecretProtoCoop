using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace Gameplay
{
    public class CallableFunctionListener : MonoBehaviour
    {
        public CallableFunction Function;
        //public GameEventPass Event2;
        public UnityEvent Response;
        private Type type;
        [HideInInspector]
        public string methodName;
        [HideInInspector]
        public Component component;
        public void OnEventRaised()
        {
            Response.Invoke();
            
        }


        private void OnEnable()
        {
            Function.RegisterListener(this);

            type = Response.GetPersistentTarget(0).GetType();
            methodName = Response.GetPersistentMethodName(0);
            component = GetComponent(type);


        }

        private void OnDisable()
        {
            Function.UnRegisterListener();
        }
    }

}
