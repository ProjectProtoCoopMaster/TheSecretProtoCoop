using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class PlayerBehavior : MonoBehaviour
    {
        public RoomManager currentRoom { get; set; }

        [SerializeField] private Vector3Variable _playerPosition;
        [SerializeField] private QuaternionVariable _playerRotation;

        [SerializeField] private GameObjectVariable _player;

        [SerializeField] private BoolVariable _hidePlayer;

        [SerializeField] private Transform playerTransform;

        void Start() => _player.Value = this.gameObject;

        void Update()
        {
            if (!_hidePlayer.Value)
            {
                if (currentRoom != null) playerTransform.position = _playerPosition.Value + (currentRoom.room.roomOrigin.position);
                playerTransform.forward = _player.Value.transform.forward;
                playerTransform.eulerAngles = new Vector3(0, playerTransform.eulerAngles.y, 0);
            }
        }
    }
}

