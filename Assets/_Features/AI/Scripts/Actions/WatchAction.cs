using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class WatchAction : ActionBehavior
    {
        [SerializeField] private float rotateSpeed;

        [SerializeField] private Transform target;

        private bool watch = false;

        private Vector3 direction;

        private Quaternion currentRotation;
        private float[] angles = new float[2];
        private float angleOffset = 1f;

        private float time;

        public override void StartActionBehavior(_Action action)
        {
            if (action.actionType == ActionType.Watch)
            {
                direction = action.watchDirection;

                SetWatch();
            }
            else if (action.actionType == ActionType.Search)
            {
                SetWatch(action.watchRotation);
            }
        }

        public override void StopActionBehavior()
        {
            watch = false;
        }

        public override bool Check()
        {
            if (target.rotation.eulerAngles.y >= angles[0] && target.rotation.eulerAngles.y <= angles[1])
            {
                watch = false;
                return true;
            }
            else return false;
        }

        #region Set
        private void SetWatch()
        {
            Vector3 _position = target.position + direction;
            Vector3 position = _position - target.position;
            position.y = 0.0f;

            currentRotation = Quaternion.LookRotation(position, Vector3.up);
            angles[0] = currentRotation.eulerAngles.y - angleOffset;
            angles[1] = currentRotation.eulerAngles.y + angleOffset;

            time = 0.0f;

            watch = true;
        }

        private void SetWatch(Vector3 angleDirection)
        {
            currentRotation = Quaternion.Euler(angleDirection);
            angles[0] = currentRotation.eulerAngles.y - angleOffset;
            angles[1] = currentRotation.eulerAngles.y + angleOffset;

            time = 0.0f;

            watch = true;
        }
        #endregion

        #region Loop
        void Update()
        {
            if (watch)
            {
                WatchDirection();
            }
        }

        private void WatchDirection()
        {
            time += (Time.deltaTime * rotateSpeed);

            target.rotation = Quaternion.Lerp(target.rotation, currentRotation, time);

            //Debug.Log(currentRotation.eulerAngles + " for " + target.parent.name);
            //Debug.Log(target.rotation.eulerAngles + " for " + target.parent.name);
        }
        #endregion
    } 
}
