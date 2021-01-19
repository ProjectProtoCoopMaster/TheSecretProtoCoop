using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class LoadNextRoom : MonoBehaviour
    {
        public bool passed;
        public DoorBehavior door;

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "[HeadCollider]" && !passed)
            {
                LevelManager.instance.ChangeRoom();

                door.Power = 0;
                door.Unlock();

                passed = true;
            }
        }
    } 
}
