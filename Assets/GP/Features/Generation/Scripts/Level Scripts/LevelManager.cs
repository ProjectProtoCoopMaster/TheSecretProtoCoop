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

        public Platform platform;

        public static LevelManager instance;
        public List<RoomManager> levelRooms { get; set; } = new List<RoomManager>();

        [ShowIf("platform", Platform.VR)]
        [Title("Level VR")]
        public LevelVR levelVR;

        [ShowIf("platform", Platform.Mobile)]
        [Title("Level Mobile")]
        public LevelMobile levelMobile;

        public Level level { get; private set; }

        void OnEnable() { if (instance == null) instance = this; }

        void Start() => onGameSceneStart.Raise(); /// LevelGenerator.GenerateLevel();

        public void StartLevel()
        {
            if (platform == Platform.VR) level = levelVR;
            else if (platform == Platform.Mobile) level = levelMobile;

            level.rooms = levelRooms;

            level.StartAt(0);
        }

        public void RestartLevel()
        {
            level.StartAt(level.currentRoomIndex);

            TransmitterManager.instance.SendLevelRestartToOthers();
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

            SetCenterToRoom(currentRoom);

            room.roomHolder.gameObject.SetActive(true);
            room.OnEnterRoom();
        }
        protected virtual void UnloadRoom(int index)
        {
            rooms[index].room.roomHolder.gameObject.SetActive(false);
            room.OnDisableRoom();
        }

        protected abstract void SetCenterToRoom(RoomManager roomManager);
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

            playerRig.position = currentRoomVR.playerStart.position;
            playerRig.rotation = currentRoomVR.playerStart.rotation;

            refreshScene.Raise();
        }

        public override void OnRoomChange()
        {
            if (currentRoomIndex >= 1) UnloadRoom(currentRoomIndex - 1);

            if (currentRoomIndex < rooms.Count - 1) LoadRoom(currentRoomIndex + 1);
            else Debug.Log("You won the game and one million pesos ! Congratulations !");

            TransmitterManager.instance.SendRoomChangeToOthers();
        }

        protected override void SetCenterToRoom(RoomManager roomManager)
        {
            playerBehavior.centerTransform = roomManager.room.roomCenter;
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
            else Debug.Log("You won the game and one million pesos ! Congratulations !");
        }

        protected override void SetCenterToRoom(RoomManager _currentRoom)
        {
            playerBehavior.currentRoom = _currentRoom;
        }
    }
}
