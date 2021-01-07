using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] private Vector3Variable _playerPosition;
        [SerializeField] private QuaternionVariable _playerRotation;
        [SerializeField] private GameObjectVariable _player;
        [SerializeField] private Transform _transform;

        private void Update()
        {
            _transform.position = _playerPosition.Value;
            _transform.forward = _player.Value.transform.forward;
            _transform.eulerAngles = new Vector3(0, _transform.eulerAngles.y, 0);
            //_transform.eulerAngles = new Vector3(0, _playerRotation.Value.eulerAngles.x, 0);


        }

    }
}

