using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Mobile
{
    public class PlayerBehvaior : MonoBehaviour
    {
        public Vector3Variable _PlayerPosition;
        public Image player;

        private void Update()
        {
            player.rectTransform.anchoredPosition = _PlayerPosition.Value;

        }
    }
}

