using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using Sirenix.OdinInspector;
using System;

namespace Networking
{
    public class MyAttribute : Attribute
    {

    }

    public class ModifiersManager : SerializedMonoBehaviour
    {
        public static ModifiersManager instance;

        public PhotonView photonView;

        public Dictionary<ModifierType, GameEvent> initEvents = new Dictionary<ModifierType, GameEvent>();

        public GameEvent shakeStart;
        public BoolVariable shake;

        private void OnEnable()
        {
            if (instance == null) instance = this;
        }

        public void Send(string name, RpcTarget targets, params object[] elements)
        {
            photonView.RPC(name, targets, elements);
        }

        [PunRPC] private void Init(ModifierType _type) => initEvents[_type].Raise();

        public void SendShakeStart() => Send("ShakeStart", RpcTarget.Others);
        [PunRPC] private void ShakeStart() => shakeStart.Raise();

        public void SendShakeResult(BoolVariable check) => Send("ShakeResult", RpcTarget.Others, check.Value);
        [PunRPC] private void ShakeResult(bool complete) => shake.Value = complete;

        public void SendVariable(params object[] variables) => Send("Variable", RpcTarget.Others, variables);
        [PunRPC] private void Variable(params object[] variables) { }
    }
}
