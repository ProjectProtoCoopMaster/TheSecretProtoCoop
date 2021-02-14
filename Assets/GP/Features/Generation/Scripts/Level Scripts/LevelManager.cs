using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Networking;
using Gameplay.VR;
using Gameplay.Mobile;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        public GameEvent onGameSceneStart;

        public Transform poolTransform;

        public LevelAssembler levelAssembler;

        public List<RoomManager> LevelRooms { get; set; } = new List<RoomManager>();

        public Platform platform;

        [ShowIf("platform", Platform.VR)]
        [Title("Level VR")]
        public LevelVR levelVR;

        [ShowIf("platform", Platform.Mobile)]
        [Title("Level Mobile")]
        public LevelMobile levelMobile;

        public Level level
        {
            get {
                if (platform == Platform.VR) return levelVR;
                else return levelMobile;
            }
        }

        public static LevelManager instance;

        void OnEnable() { if (instance == null) instance = this; }

        void Start()
        {
            onGameSceneStart.Raise(); /// LevelGenerator.GenerateLevel();
            poolTransform.gameObject.SetActive(false);
        }

        public void BuildLevel() => levelAssembler.AssembleLevel();

        public void StartLevel()
        {
            level.rooms = LevelRooms;

            level.StartAt(0);
        }

        public void RestartLevel()
        {
            level.StartAt(level.currentRoomIndex);
        }

        public void ChangeRoom() => level.OnRoomChange();
    }

    [System.Serializable]
    public abstract class Level
    {
        public List<RoomManager> rooms { get; set; } = new List<RoomManager>();

        public int currentRoomIndex { get; protected set; }
        public RoomManager currentRoom { get; protected set; }

        public Room room { get => currentRoom.room; }

        public abstract void StartAt(int roomIndex);

        public abstract void OnRoomChange();

        protected virtual void LoadRoom(int index)
        {
            currentRoomIndex = index;
            currentRoom = rooms[currentRoomIndex];

            SetPlayerRoom(currentRoom);

            room.transform.gameObject.SetActive(true);
            room.OnEnterRoom();
        }
        protected virtual void UnloadRoom(int index)
        {
            rooms[index].room.transform.gameObject.SetActive(false);
            room.OnDisableRoom();
        }

        protected abstract void SetPlayerRoom(RoomManager _currentRoom);
    }

    [System.Serializable]
    [HideLabel]
    public class LevelVR : Level
    {
        public GameEvent refreshScene;

        public Transform playerRig;
        public VR.PlayerBehavior playerBehavior;

        public RoomVR currentRoomVR { get => (RoomVR)currentRoom.room; }

        public override void StartAt(int roomIndex)
        {
            LoadRoom(roomIndex);

            playerBehavior.ResetPlayer();

            playerRig.position = currentRoomVR.playerStart.position;
            playerRig.rotation = currentRoomVR.playerStart.rotation;

            refreshScene.Raise();
        }

        public override void OnRoomChange()
        {
            if (currentRoomIndex >= 1) UnloadRoom(currentRoomIndex - 1);

            if (currentRoomIndex < rooms.Count - 1) LoadRoom(currentRoomIndex + 1);

            else TransmitterManager.instance.SendWinToAll();
        }

        protected override void SetPlayerRoom(RoomManager _currentRoom)
        {
            playerBehavior.currentRoom = _currentRoom;
        }
    }

    [System.Serializable]
    [HideLabel]
    public class LevelMobile : Level
    {
        public Mobile.PlayerBehavior playerBehavior;

        public override void StartAt(int roomIndex) => LoadRoom(roomIndex);

        public override void OnRoomChange()
        {
            if (currentRoomIndex >= 0) UnloadRoom(currentRoomIndex);

            if (currentRoomIndex < rooms.Count - 1) LoadRoom(currentRoomIndex + 1);
        }

        protected override void SetPlayerRoom(RoomManager _currentRoom)
        {
            playerBehavior.currentRoom = _currentRoom;
        }
    }
}
