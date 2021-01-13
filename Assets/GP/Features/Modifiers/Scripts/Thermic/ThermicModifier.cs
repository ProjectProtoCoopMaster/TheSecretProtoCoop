using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class ThermicModifier : Modifier
    {
        [SerializeField] private BoolVariable _hidePlayer;
        [SerializeField] private CallableFunction _sendHidePlayer;
        //private void Start()
        //{
        //    Init();
        //}

        public override void Init()
        {
            _hidePlayer.Value = true;
            _sendHidePlayer.Raise(_hidePlayer.Value);
            base.Init();
        }

        public override void End()
        {
            _hidePlayer.Value = false;
            _sendHidePlayer.Raise(_hidePlayer.Value);
            base.End();
        }

        private void OnDisable()
        {
            End();
        }
    } 
}
