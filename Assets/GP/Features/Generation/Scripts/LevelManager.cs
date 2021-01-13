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
        public List<RoomManager> levelRooms { get; private set; } = new List<RoomManager>();

        public Level level { get; private set; }

        private void OnEnable()
        {
            if (instance == null) instance = this;
        }

        public void StartLevel()
        {
            if (platform == Platform.VR) level = new LevelVR();
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

        public abstract void Start();

        public virtual void OnRoomChange()
        {
            if (currentRoomIndex >= 1) UnloadRoom(currentRoomIndex - 1);

            if (currentRoomIndex < rooms.Count - 1) LoadRoom(currentRoomIndex + 1);
            else Debug.Log("You won the game and one million pesos ! Congratulations !");
        }

        protected abstract void LoadRoom(int index);
        protected abstract void UnloadRoom(int index);
    }

    public class LevelVR : Level
    {
        public GameObject playerRig;

        public RoomVR currentRoomVR { get => (RoomVR)currentRoom.room; }

        public override void Start()
        {
            LoadRoom(0);

            playerRig.transform.position = currentRoomVR.playerStart.position;
            playerRig.transform.rotation = currentRoomVR.playerStart.rotation;
        }

        public override void OnRoomChange()
        {
            base.OnRoomChange();

            // Send Event to the Network --> OnRoomChange Mobile
        }

        protected override void LoadRoom(int index)
        {
            currentRoomIndex = index;
            currentRoom = rooms[currentRoomIndex];

            currentRoomVR.parent.gameObject.SetActive(true);
            currentRoomVR.OnEnterRoom();
        }
        protected override void UnloadRoom(int index)
        {
            RoomVR indexRoom = (RoomVR)rooms[index].room;
            indexRoom.parent.gameObject.SetActive(false);
            currentRoomVR.OnDisableRoom();
        }
    }
    public class LevelMobile : Level
    {
        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        protected override void LoadRoom(int index)
        {
            throw new System.NotImplementedException();
        }
        protected override void UnloadRoom(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
