using Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class LoadNextRoom : MonoBehaviour
    {
        public bool passed { get; set; }

        public DoorBehavior door;

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "[HeadCollider]" && !passed)
            {
                TransmitterManager.instance.SendRoomChangeToAll();

                door.Power = 0;
                door.Unlock();

                passed = true;
            }
        }
    } 
}
