using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class WaitAction : ActionBehavior
    {
        private bool wait;

        private float currentTime;

        public override void StartActionBehavior(_Action action)
        {
            currentTime = action.timeToWait;
            wait = true;
        }

        public override void StopActionBehavior()
        {
            wait = false;
        }

        public override bool Check()
        {
            if (currentTime <= 0.0f)
            {
                wait = false;
                return true;
            }
            else return false;
        }

        void Update()
        {
            if (wait)
            {
                currentTime -= Time.deltaTime;
            }
        }
    } 
}
