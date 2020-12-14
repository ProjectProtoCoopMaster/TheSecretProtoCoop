using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Mobile
{
    public class PlayerBehavior : MonoBehaviour
    {
        public Vector3Variable _playerPosition;
        public QuaternionVariable _playerRotation;
        public Image player;
        //private Quaternion currentRot;


        private void Update()
        {
            player.rectTransform.anchoredPosition = _playerPosition.Value;
            player.rectTransform.eulerAngles = new Vector3(0,0,-_playerRotation.Value.eulerAngles.y +90);

        }
    }
}

