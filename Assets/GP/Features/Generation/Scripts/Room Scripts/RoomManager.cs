using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Networking;
using Photon.Pun;
using Gameplay.AI;
using Gameplay.Mobile;

namespace Gameplay
{
    public abstract class Modifier : MonoBehaviour
    {
        public bool active { get; set; }
        public bool check { get; protected set; }

        [Button]
        public virtual void Init() => active = true;
        [Button]
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

        public void StartRoom()
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

        public Transform roomHolder;

        public Transform roomCenter;
        public Transform roomOrigin;

        public Transform LocalPlayer;

        public ModifierType roomModifier { get; set; } = ModifierType.None;

        protected bool init = true;

        public virtual void OnInitRoom() => init = false;

        public virtual void OnEnterRoom() { if (init) OnInitRoom(); }
        public virtual void OnDisableRoom() { }
    }

    [System.Serializable]
    [HideLabel]
    public class RoomVR : Room
    {
        public NavMeshSurface roomNavigationSurface;

        public AIManager aIManager;
        public List<Vector3> aIPositions { get; set; }
        public List<Quaternion> aIRotations { get; set; }

        public Transform entranceAnchor;
        public Transform exitAnchor;

        public Transform playerStart;

        public override void OnInitRoom()
        {
            /// Initialize AI

            aIPositions = new List<Vector3>();
            aIRotations = new List<Quaternion>();

            for (int i = 0; i < AIManager.agents.Count; i++)
            {
                aIPositions.Add(AIManager.agents[i].transform.position);
                aIRotations.Add(AIManager.agents[i].transform.rotation);
            }

            base.OnInitRoom();
        }

        public override void OnEnterRoom()
        {
            base.OnEnterRoom();

            /// Initialize Modifier

            //if (roomModifier != ModifierType.None) ModifiersManager.instance.Send("Init", RpcTarget.All, roomModifier);

            /// Initialize AI

            for (int i = 0; i < AIManager.agents.Count; i++)
            {
                AIManager.agents[i].DeletePreviousRagdoll();
                AIManager.agents[i].agentRig.gameObject.SetActive(true);

                AIManager.agents[i].transform.position = aIPositions[i];
                AIManager.agents[i].transform.rotation = aIRotations[i];
            }

            roomNavigationSurface.BuildNavMesh();

            aIManager.StartAllAgents();

            /// Initialize Elements
            
            SwitcherManager.instance.StartAllSwitchers();
            JammerManager.instance.StartAllJammers();

            SymbolManager.instance.LoadSymbols();
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
        public Canvas canvas;

        public float width = 10f;
        public float height = 10f;

        private CameraManager cameraManager;

        public override void OnInitRoom()
        {
            /// Initialize Camera

            cameraManager = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();

            base.OnInitRoom();
        }

        public override void OnEnterRoom()
        {
            base.OnEnterRoom();

            /// Initialize Camera

            cameraManager.SetCamera(width, height, roomCenter);
            canvas.worldCamera = cameraManager._camera;

            Vector3 canvasPosition = new Vector3(roomCenter.position.x, 5f, roomCenter.position.z);
            canvas.transform.position = canvasPosition;

            /// Initialize Switchers

            SwitcherManager.instance.StartAllSwitchers();
            JammerManager.instance.StartAllJammers();
        }
    }
}
