using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Networking;

namespace Gameplay.VR
{
    public class LoadNextLevel : MonoBehaviour
    {
        public bool passed { get; set; }

        [SerializeField] private CallableFunction _sendLoadNextRoom;

        void OnTriggerEnter(Collider other)
        {
            if (other.name == "[HeadCollider]" && !passed)
            {
                if (GameManager.instance.currentLevelIndex < 3)
                {
                    Debug.Log("a la zeub" + GameManager.instance.currentLevelIndex);

                    LoadNextScene();
                    passed = true;
                }
                else
                {
                    TransmitterManager.instance.SendWinToAll();
                }
            }
        }

        [Button]
        public void LoadNextScene()
        {
            _sendLoadNextRoom.Raise();
        }
    }
}
