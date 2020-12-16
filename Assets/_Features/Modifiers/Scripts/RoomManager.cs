using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Networking;
using Photon.Pun;

namespace Gameplay
{
    public abstract class Modifier : MonoBehaviour
    {
        public bool active { get; set; }
        public bool check { get; protected set; }

        public virtual void Init() => active = true;

        public virtual void End() => active = false;
    }

    public class RoomManager : MonoBehaviour
    {
        public bool useLevelGeneration;

        [ShowIf("useLevelGeneration")] public Transform entranceAnchor;
        [ShowIf("useLevelGeneration")] public Transform exitAnchor;

        public ModifierType roomModifier = ModifierType.None;

        void Start()
        {
            OnEnterRoom();
        }

        public void OnEnterRoom()
        {
            if (roomModifier != ModifierType.None)
            {
                ModifiersManager.instance.Send("Init", RpcTarget.All, roomModifier);
            }
        }
    } 
}
