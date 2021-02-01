using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;

namespace Gameplay
{
    public enum ModifierType { DarkZone = 0, Thermic = 1, Oxygen = 2, None = 3 }

    public class LevelGenerator : SerializedMonoBehaviour
    {
        public LevelFile levelFile;

        [Range(0, 3)]
        public int maximumModifiersInLevel;
        
        private List<ModifierType> modifierTypes = new List<ModifierType> { ModifierType.DarkZone, ModifierType.Thermic, ModifierType.Oxygen };

        public LevelVariable levelHolder;

        public CallableFunction sendLevelHolder;

        public BoolVariable isMobile;

        public void GenerateLevel() /// OnGameSceneStart Event
        {
            if (!isMobile.Value)
            {
                Debug.Log("Generate");
                levelHolder.LevelRoomsData.Clear();

                SelectRooms();

                ApplyModifiers();

                // Build the Level in Mobile
                TransmitterManager.instance.SendBuildLevelToOther(levelHolder);
                // Build the Level in VR
                LevelManager.instance.BuildLevel();
            }
        }

        #region Rooms
        private void SelectRooms()
        {
            foreach (PoolData pool in levelFile.pools.Values)
            {
                PickRoom(pool);
            }
        }
        private void PickRoom(PoolData pool)
        {
            List<RoomData> availableRooms = new List<RoomData>();
            foreach (RoomData room in pool.rooms) availableRooms.Add(room);

            int amountToPick;
            if (availableRooms.Count < pool.amountOfRoomsToPick) amountToPick = availableRooms.Count;
            else amountToPick = pool.amountOfRoomsToPick;

            int pick;

            for (int p = 0; p < amountToPick; p++)
            {
                pick = Random.Range(0, availableRooms.Count);

                levelHolder.LevelRoomsData.Add(availableRooms[pick]);

                availableRooms.RemoveAt(pick);
            }
        }
        #endregion

        #region Modifiers
        private void ApplyModifiers()
        {
            int modifiersAmount = Random.Range(0, maximumModifiersInLevel) + 1;
            int[] modifiedRooms = new int[modifiersAmount];

            List<RoomData> unmodifiedRooms = new List<RoomData>();
            foreach (RoomData room in levelHolder.LevelRoomsData) unmodifiedRooms.Add(room);

            for (int r = 0; r < modifiedRooms.Length; r++)
            {
                modifiedRooms[r] = Random.Range(0, unmodifiedRooms.Count);

                int m = Random.Range(0, modifierTypes.Count);
                ModifierType modifier = modifierTypes[m];

                unmodifiedRooms[modifiedRooms[r]].roomModifier = modifier;

                modifierTypes.RemoveAt(m);

                unmodifiedRooms.RemoveAt(modifiedRooms[r]);
            }
            unmodifiedRooms.Clear();
        }
        #endregion
    }
}
