using System.Collections;
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
        public string roomName;

        [InfoBox(@"@""Modifier : "" + this.roomModifier.ToString()")]

        public Transform entranceAnchor;
        public Transform exitAnchor;

        public Transform playerStart;

        public Transform roomParent;

        public ModifierType roomModifier { get; set; } = ModifierType.None;

        public NavMeshSurface roomNavigationSurface;

        public AIManager aIManager;

        public void OnEnterRoom()
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

        public void OnDisableRoom()
        {
            if (roomNavigationSurface != null) roomNavigationSurface.RemoveData();
        }
    }

    public class Room
    {

    }

    public class RoomVR : Room
    {

    }
    public class RoomMobile : Room
    {

    }
}
