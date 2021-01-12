using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class LevelGenerator : SerializedMonoBehaviour
    {
        public static LevelGenerator instance;

        public GameObject playerRig = null;

        public Transform levelEntranceAnchor;

        public List<RoomManager> levelRooms { get; private set; } = new List<RoomManager>();

        public int currentRoomIndex { get; private set; }
        public RoomManager currentRoom { get; private set; }

        private void OnEnable()
        {
            if (instance == null) instance = this;
        }

        #region Transition
        public void OnRoomEnd()
        {
            // Unload Previous Room
            if (currentRoomIndex >= 1) UnloadRoom(currentRoomIndex - 1);

            // Load Next Room
            if (currentRoomIndex < levelRooms.Count - 1) LoadRoom(currentRoomIndex + 1);
            // Win the Game
            else Debug.Log("You won the game and one million pesos ! Congratulations !");
        }

        private void LoadRoom(int index)
        {
            currentRoomIndex = index;
            currentRoom = levelRooms[currentRoomIndex];

            currentRoom.roomParent.gameObject.SetActive(true);
            currentRoom.OnEnterRoom();
        }
        private void UnloadRoom(int index)
        {
            levelRooms[index].roomParent.gameObject.SetActive(false);
            currentRoom.OnDisableRoom();
        }
        #endregion

        #region Initialization
        void Start()
        {
            // Generate a new Procedural Level
            GenerateLevel();

            // Load the first Room of the Level
            LoadRoom(0);

            string msg = "There is no Player attached to the Level Manager. Attach one to initialize the player's position in the level !";
            if (Utility.SafeCheck(playerRig, msg))
            {
                // Sets the Player Area Position at the Entrance of the Room
                playerRig.transform.position = currentRoom.playerStart.position;
                playerRig.transform.rotation = currentRoom.playerStart.rotation;
            }
        }

        private void GenerateLevel()
        {
            foreach (RoomManager room in levelRooms) room.roomParent.gameObject.SetActive(false);

            CreateLevel();
        }
        #endregion

        #region Level Creation
        private void CreateLevel()
        {
            Transform currentAnchor = levelEntranceAnchor;

            for (int i = 0; i < levelRooms.Count; i++)
            {
                Vector3 translation = currentAnchor.position - levelRooms[i].entranceAnchor.localPosition;
                levelRooms[i].roomParent.gameObject.transform.position = translation;

                float angle = currentAnchor.rotation.eulerAngles.y - levelRooms[i].entranceAnchor.localRotation.eulerAngles.y;
                levelRooms[i].roomParent.gameObject.transform.RotateAround(currentAnchor.position, Vector3.up, angle);

                levelRooms[i].roomParent.gameObject.transform.parent = this.transform;

                currentAnchor = levelRooms[i].exitAnchor;
            }
        }
        #endregion
    }
}

