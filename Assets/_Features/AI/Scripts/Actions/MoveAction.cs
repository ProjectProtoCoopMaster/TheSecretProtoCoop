using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.AI
{
    public class MoveAction : ActionBehavior
    {
        public Transform target;

        public Vector3 destination { get; set; }
        public float area { get; set; }

        public override void StartActionBehavior(_Action action)
        {
            area = action.area;
            SetMove(action.destination, true);
        }

        public override void StopActionBehavior()
        {
            Debug.Log(this);
            SetMove(target.position, false);
        }

        public override bool Check()
        {
            if (IsInArea(target.position, destination, area))
            {
                SetMove(target.position, false);

                return true;
            }
            else return false;
        }

        public virtual void SetMove(Vector3 direction, bool move)
        {
            destination = direction;
        }

        public bool IsInArea(Vector3 objectPos, Vector3 destPos, float area)
        {
            if (objectPos.x <= (destPos.x + area) && objectPos.x >= (destPos.x - area))
            {
                if (objectPos.z <= (destPos.z + area) && objectPos.z >= (destPos.z - area))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }
    } 
}
