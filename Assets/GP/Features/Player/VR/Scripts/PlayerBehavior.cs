using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerBehavior : MonoBehaviour, IKillable
    {
        public Transform centerTransform { get; set; }

        [SerializeField] private Transform _transform;

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

        private void Update()
        {
            _playerRotation.Value = _transform.localRotation;
            _playerPosition.Value = _transform.position; /*- centerTransform.position;*/

            _player.Value = this.gameObject;

            _sendPlayerPosAndRot.Raise();
        }
    }
}

