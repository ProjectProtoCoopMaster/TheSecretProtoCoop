using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public class LevelManager : MonoBehaviour
    {
        public Platform platform;

        public static LevelManager instance;
        public List<RoomManager> levelRooms { get; set; } = new List<RoomManager>();

        public Level level { get; private set; }

        public GameEvent onGameSceneVRStart;

        public Transform _playerRig;

        private void OnEnable()
        {
            if (instance == null) instance = this;
        }

        private void Start()
        {
            onGameSceneVRStart.Raise();
        }

        public void StartLevel()
        {
            if (platform == Platform.VR) level = new LevelVR { playerRig = _playerRig };
            else if (platform == Platform.Mobile) level = new LevelMobile();

            level.rooms = levelRooms;

            level.Start();
        }
    }

    public abstract class Level
    {
        public List<RoomManager> rooms { get; set; } = new List<RoomManager>();

        public int currentRoomIndex { get; protected set; }
        public RoomManager currentRoom { get; protected set; }

        public Room room { get => currentRoom.room; }

        public abstract void Start();

        public abstract void OnRoomChange();

        protected void LoadRoom(int index)
        {
            currentRoomIndex = index;
            currentRoom = rooms[currentRoomIndex];

            room.parent.gameObject.SetActive(true);
            room.OnEnterRoom();
        }
        protected void UnloadRoom(int index)
        {
            rooms[index].room.parent.gameObject.SetActive(false);
            room.OnDisableRoom();
        }
    }

    public class LevelVR : Level
    {
        public Transform playerRig;

        public RoomVR currentRoomVR { get => (RoomVR)currentRoom.room; }

        public override void Start()
        {
            LoadRoom(0);

            playerRig.position = currentRoomVR.playerStart.position;
            playerRig.rotation = currentRoomVR.playerStart.rotation;
        }

        public override void OnRoomChange()
        {
            if (currentRoomIndex >= 1) UnloadRoom(currentRoomIndex - 1);

            if (currentRoomIndex < rooms.Count - 1) LoadRoom(currentRoomIndex + 1);
            else Debug.Log("You won the game and one million pesos ! Congratulations !");

            // Send Event to the Network --> OnRoomChange Mobile
        }
    }
    public class LevelMobile : Level
    {
        public override void Start() => LoadRoom(0);

        public override void OnRoomChange()
        {
            if (currentRoomIndex >= 0) UnloadRoom(currentRoomIndex);

            if (currentRoomIndex < rooms.Count - 1) LoadRoom(currentRoomIndex + 1);
            else Debug.Log("You won the game and one million pesos ! Congratulations !");
        }
    }
}
