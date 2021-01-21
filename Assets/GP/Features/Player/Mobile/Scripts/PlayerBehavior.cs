using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    public class PlayerBehavior : MonoBehaviour
    {
        public Transform centerTransform { get; set; }

        [SerializeField] private Vector3Variable _playerPosition;
        [SerializeField] private QuaternionVariable _playerRotation;

        [SerializeField] private GameObjectVariable _player;

        [SerializeField] private BoolVariable _hidePlayer;

        [SerializeField] private Transform _transform;


        void Start()
        { 
            _player.Value = this.gameObject;
            _transform.localScale = new Vector3(2, 2, 2);
        
        }

        void Update()
        {
            if (!_hidePlayer.Value)
            {
                _transform.position = _playerPosition.Value; /*+ centerTransform.position;*/
                //_transform.forward = _player.Value.transform.forward;
                
                transform.rotation = _playerRotation.Value;
                transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, 0);
            }
        }
    }
}

