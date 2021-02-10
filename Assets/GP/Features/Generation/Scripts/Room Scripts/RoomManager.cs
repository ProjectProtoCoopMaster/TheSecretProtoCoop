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
        public GameEvent _refreshScene;

        public Platform platform;

        [ShowIf("platform", Platform.VR)]
        [Title("Room VR")]
        public RoomVR roomVR;

        [ShowIf("platform", Platform.Mobile)]
        [Title("Room Mobile")]
        public RoomMobile roomMobile;

        public Room room
        {
            get {
                if (platform == Platform.VR) return roomVR;
                else return roomMobile;
            }
        }

        private void Awake()
        {
            _refreshScene.Raise();
        }
    }

    [System.Serializable]
    public abstract class Room
    {
        public string roomName;

        [InfoBox(@"@""Modifier : "" + this.roomModifier.ToString()")]

        public Transform transform;

        public Transform roomCenter;
        public Transform roomOrigin;

        public Transform LocalPlayer;

        public ModifierType roomModifier { get; set; } = ModifierType.None;

        protected bool init = true;

        protected virtual void OnInitRoom() => init = false;

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

        protected override void OnInitRoom()
        {
            /// Initialize AI

            Debug.Log("setting guards");

            aIPositions = new List<Vector3>();
            aIRotations = new List<Quaternion>();

            for (int i = 0; i < aIManager.Agents.Count; i++)
            {
                aIPositions.Add(aIManager.Agents[i].transform.position);
                aIRotations.Add(aIManager.Agents[i].transform.rotation);
            }

            base.OnInitRoom();
        }

        public override void OnEnterRoom()
        {
            base.OnEnterRoom();

            /// Initialize Modifier

            //if (roomModifier != ModifierType.None) ModifiersManager.instance.Send("Init", RpcTarget.All, roomModifier);

            /// Initialize AI

            for (int i = 0; i < aIManager.Agents.Count; i++)
            {
                Debug.Log("resetting guards");

                aIManager.Agents[i].DeletePreviousRagdoll();
                aIManager.Agents[i].agentRig.gameObject.SetActive(true);

                aIManager.Agents[i].transform.position = aIPositions[i];
                aIManager.Agents[i].transform.rotation = aIRotations[i];
            }

            roomNavigationSurface.BuildNavMesh();

            foreach (AgentManager agent in aIManager.Agents) { agent.navMeshAgent.enabled = false; agent.navMeshAgent.enabled = true; }

            aIManager.StartAllAgents();

            /// Initialize Elements

            TransmitterManager.instance.switcherManager.StartAllSwitchers();
            TransmitterManager.instance.jammerManager.StartAllJammers();

            TransmitterManager.instance.symbolManager.LoadSymbols();
        }

        public override void OnDisableRoom()
        {
            var Yes = new { myMan = "Boi" };
            var type = Yes.GetType();
            Debug.Log(type.Name);

            //if (roomNavigationSurface != null) roomNavigationSurface.RemoveData();
        }
    }

    public class Bunker
    {
        /// Real Singleton Pattern
        private Bunker() { }
        public static Bunker instance { get; } = new Bunker();
    }

    [System.Serializable]
    [HideLabel]
    public class RoomMobile : Room
    {
        public Canvas canvas;

        public float width = 10f;
        public float height = 10f;

        private CameraManager cameraManager;

        protected override void OnInitRoom()
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

            TransmitterManager.instance.switcherManager.StartAllSwitchers();
            TransmitterManager.instance.jammerManager.StartAllJammers();
        }
    }
}
