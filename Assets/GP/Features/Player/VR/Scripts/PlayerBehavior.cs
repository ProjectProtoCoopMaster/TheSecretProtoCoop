using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerBehavior : MonoBehaviour, IKillable
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private GameEvent playerHitTrap, raiseAlarm;

        [SerializeField] private CallableFunction _gameOver;
        [SerializeField] private CallableFunction _sendPlayerPosAndRot;
        [SerializeField] private Vector3Variable _playerPosition;
        [SerializeField] private QuaternionVariable _playerRotation;
        [SerializeField] private GameObjectVariable _player;
        //[SerializeField] private GameObjectVariable pictureCameraObj;
        [SerializeField] private Camera pictureCamera;
        private bool isDead;
        public float debugRot;
     
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
            //_playerPosition.Value = pictureCamera.WorldToScreenPoint(new Vector3(transform.position.x, 0, transform.position.z));
            _playerRotation.Value = _transform.localRotation;
            _playerPosition.Value = _transform.position;
            _player.Value = this.gameObject;
            //_playerRotation.Value.eulerAngles = new Vector3(_playerRotation.Value.eulerAngles.x, _playerRotation.Value.eulerAngles.y+ debugRot, _playerRotation.Value.eulerAngles.z);
            _sendPlayerPosAndRot.Raise();
        }
    }
}

