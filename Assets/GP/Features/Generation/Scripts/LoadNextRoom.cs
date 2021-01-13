using Gameplay.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LoadNextRoom : MonoBehaviour
    {
        public bool passed;
        public DoorBehavior door;

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "[HeadCollider]" && !passed)
            {
                LevelManager.instance.level.OnRoomChange();

                door.Power = 0;
                door.Unlock();

                passed = true;
            }
        }
    } 
}
