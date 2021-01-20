using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerBehavior : MonoBehaviour, IKillable
    {
        public RoomManager currentRoom { get; set; }

        [SerializeField] private Transform rigTransform;

        [SerializeField] private GameEvent playerHitTrap, raiseAlarm;

        [SerializeField] private CallableFunction _gameOver;

        [SerializeField] private CallableFunction _sendPlayerPosAndRot;

        [SerializeField] private Vector3Variable _playerPosition;
        [SerializeField] private QuaternionVariable _playerRotation;

        [SerializeField] private GameObjectVariable _player;

        private bool isDead;

        public void Die(Vector3 direction = default)
        {
            if (!isDead)
            {
                raiseAlarm.Raise();
                _gameOver.Raise();
                playerHitTrap.Raise();

                isDead = true;
            }
        }

        public void Die()
        {
            raiseAlarm.Raise();
            playerHitTrap.Raise();
        }

        void Update()
        {
            _playerRotation.Value = rigTransform.localRotation;
            if (currentRoom != null) _playerPosition.Value = rigTransform.position - (currentRoom.room.roomOrigin.position + currentRoom.room.roomOrigin.localPosition);

            //_player.Value = this.gameObject;

            _sendPlayerPosAndRot.Raise();
        }
    }
}
