using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Networking;

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

            level.Start();
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

        public abstract void Start();

        public abstract void OnRoomChange();

        protected void LoadRoom(int index)
        {
            currentRoomIndex = index;
            currentRoom = rooms[currentRoomIndex];

            room.roomHolder.gameObject.SetActive(true);
            room.OnEnterRoom();
        }
        protected void UnloadRoom(int index)
        {
            rooms[index].room.roomHolder.gameObject.SetActive(false);
            room.OnDisableRoom();
        }
    }

    [System.Serializable]
    [HideLabel]
    public class LevelVR : Level
    {
        public GameEvent refreshScene;

        public Transform playerRig;

        public RoomVR currentRoomVR { get => (RoomVR)currentRoom.room; }

        public override void Start()
        {
            LoadRoom(0);

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
    }

    [System.Serializable]
    [HideLabel]
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
