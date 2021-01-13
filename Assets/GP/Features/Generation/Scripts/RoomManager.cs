﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Networking;
using Photon.Pun;
using Gameplay.AI;

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
        public Platform platform;

        [ShowIf("platform", Platform.VR)]
        [Title("Room VR")]
        public RoomVR roomVR;
        [ShowIf("platform", Platform.Mobile)]
        [Title("Room Mobile")]
        public RoomMobile roomMobile;

        public Room room { get; private set; }

        void Start()
        {
            if (platform == Platform.VR) room = roomVR;
            else if (platform == Platform.Mobile) room = roomMobile;
        }
    }

    [System.Serializable]
    public abstract class Room
    {
        public string roomName;

        [InfoBox(@"@""Modifier : "" + this.roomModifier.ToString()")]

        public Transform parent;

        public ModifierType roomModifier { get; set; } = ModifierType.None;

        public abstract void OnEnterRoom();
        public abstract void OnDisableRoom();
    }

    [System.Serializable]
    [HideLabel]
    public class RoomVR : Room
    {
        public NavMeshSurface roomNavigationSurface;

        public AIManager aIManager;

        public Transform entranceAnchor;
        public Transform exitAnchor;

        public Transform playerStart;

        public override void OnEnterRoom()
        {
            // Initialize Modifier
            //if (roomModifier != ModifierType.None) ModifiersManager.instance.Send("Init", RpcTarget.All, roomModifier);

            // Bake NavMesh
            string navMsg = "There is no NavMesh Surface attached to the Room Manager, attach one to initialize this room's Navigation Mesh";
            if (Utility.SafeCheck(roomNavigationSurface, navMsg))
            {
                roomNavigationSurface.BuildNavMesh();
            }

            // Initialization AI
            string aiMsg = "There's no AI Manager attached to the Room Manager, attach one to initialize this room's AI";
            if (Utility.SafeCheck(aIManager, aiMsg))
            {
                aIManager.StartAllAgents();
            }

            // Initialization Switchers
            // Initialize Elements
        }

        public override void OnDisableRoom()
        {
            if (roomNavigationSurface != null) roomNavigationSurface.RemoveData();
        }
    }

    [System.Serializable]
    [HideLabel]
    public class RoomMobile : Room
    {
        public override void OnEnterRoom()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDisableRoom()
        {
            throw new System.NotImplementedException();
        }
    }
}
