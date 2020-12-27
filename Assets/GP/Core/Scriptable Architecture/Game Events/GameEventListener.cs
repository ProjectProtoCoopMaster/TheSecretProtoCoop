using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        //public GameEventPass Event2;
        public UnityEvent Response;

        public void OnEventRaised()
        {
            Response.Invoke();
        }

        /*public void OnEventObjectRaised()
        {
            Event2.Invoke();
        }*/

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnRegisterListener(this);
        }
    }
}

