using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerBehavior : MonoBehaviour, IKillable
    {
        [SerializeField] private GameEvent playerHitTrap;

        [SerializeField] private CallableFunction _gameOver;
        [SerializeField] private CallableFunction _sendPlayerPosAndRot;
        [SerializeField] private Vector3Variable _playerPosition;
        [SerializeField] private QuaternionVariable _playerRotation;
        [SerializeField] private Camera pictureCamera;
        private bool isDead;

        public void Die(Vector3 direction = default)
        {
            if (!isDead)
            {
                _gameOver.Raise();
                playerHitTrap.Raise();
                isDead = true;
            }
        }

        public void Die()
        {
            playerHitTrap.Raise();
        }

        private void Update()
        {
            _playerPosition.Value = pictureCamera.WorldToScreenPoint(new Vector3(transform.position.x, 0, transform.position.z));
            _playerRotation.Value = transform.localRotation;
            _sendPlayerPosAndRot.Raise();
        }
    }
}

