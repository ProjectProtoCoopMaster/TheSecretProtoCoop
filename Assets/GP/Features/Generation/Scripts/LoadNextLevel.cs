using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay.Mobile
{
    public class LoadNextLevel : MonoBehaviour
    {
        public bool passed { get; set; }

        [SerializeField] private CallableFunction _sendLoadNextRoom;
        void OnTriggerEnter(Collider other)
        {
            if (other.name == "[HeadCollider]" && !passed)
            {

                LoadNextScene();
                passed = true;
            }
        }
        [Button]
        public void LoadNextScene()
        {
            _sendLoadNextRoom.Raise();
        }
    }
}

