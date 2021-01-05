using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        public Transform entranceAnchor;
        public Transform exitAnchor;

        public ModifierType roomModifier = ModifierType.None;

        public NavMeshSurface roomNavigationSurface;

        void Start()
        {
            OnEnterRoom();
        }

        public void OnEnterRoom()
        {
            // Initialize Modifier
            //if (roomModifier != ModifierType.None) ModifiersManager.instance.Send("Init", RpcTarget.All, roomModifier);

            // Bake NavMesh
            roomNavigationSurface.BuildNavMesh();
            // Initialization AI


            // Initialization Switchers
                // Initialize Elements
        }
    } 
}
