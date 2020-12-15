using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using Sirenix.OdinInspector;

namespace Networking
{
    public class ModifiersManager : SerializedMonoBehaviour
    {
        public PhotonView photonView;

        public Dictionary<ModifierType, GameEvent> initEvents = new Dictionary<ModifierType, GameEvent>();

        public void InitializeModifier(ModifierType type) => photonView.RPC("Init", RpcTarget.All, type);
        [PunRPC] private void Init(ModifierType _type) => initEvents[_type].Raise();
    }
}
