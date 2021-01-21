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
            // Rotation
            _playerRotation.Value = rigTransform.localRotation;

            // Position
            if (currentRoom != null)
            {
                // Sets the Position of the Player in the Room to the position of the Player in the World
                currentRoom.room.LocalPlayer.position = rigTransform.position;

                // Sets the Position Variable to the Local Position of the Player (Relative to the Room)
                _playerPosition.Value = currentRoom.room.LocalPlayer.localPosition;
            }

            _sendPlayerPosAndRot.Raise();
        }
    }
}
