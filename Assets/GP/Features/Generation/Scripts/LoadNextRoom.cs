using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LoadNextRoom : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            LevelManager level = LevelManager.instance;
            if (other.gameObject.layer == 8) level.LoadRoom(level.currentRoomIndex + 1);
        }
    } 
}
