using Gameplay.VR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LoadNextRoom : MonoBehaviour
    {
        public DoorBehavior door;

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "[HeadCollider]")
            {
                LevelManager.instance.OnRoomEnd();

                door.Power = 0;
                door.Unlock();
            }
        }
    } 
}
