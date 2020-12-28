using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public enum ModifierType { DarkZone, Thermic, Oxygen, None }

    [System.Serializable]
    public class Pool
    {
        public int amountOfRoomsToPick;
        public List<RoomManager> rooms = new List<RoomManager>();
    }

    public class LevelManager : SerializedMonoBehaviour
    {
        public Transform levelEntranceAnchor;

        public int maximumModifiersInLevel;

        public Pool easyPool, mediumPool, hardPool;

        private List<RoomManager> levelRooms = new List<RoomManager>();

        public Dictionary<ModifierType, Modifier> modifiers;
        private List<ModifierType> modifierTypes;

        void Start()
        {
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            modifierTypes = new List<ModifierType> { ModifierType.DarkZone, ModifierType.Thermic, ModifierType.Oxygen };

            List<Pool> pools = new List<Pool> { easyPool, mediumPool, hardPool };
            SelectRooms(pools);

            ApplyModifiers();

            CreateLevel();
        }

        private void SelectRooms(List<Pool> pools)
        {
            foreach (Pool pool in pools)
            {
                PickRoom(pool);
            }
        }

        private void PickRoom(Pool pool)
        {
            List<RoomManager> availableRoomsInPool = new List<RoomManager>();
            foreach (RoomManager room in pool.rooms) availableRoomsInPool.Add(room);

            int pick;

            for (int p = 0; p < pool.amountOfRoomsToPick; p++)
            {
                pick = Random.Range(0, availableRoomsInPool.Count);

                levelRooms.Add(availableRoomsInPool[pick]);

                availableRoomsInPool.RemoveAt(pick);
            }
        }

        private void ApplyModifiers()
        {
            int modifiersAmount = Random.Range(0, maximumModifiersInLevel) + 1;
            int[] modifiedRooms = new int[modifiersAmount];

            List<RoomManager> unmodifiedRooms = new List<RoomManager>();
            foreach (RoomManager room in levelRooms) unmodifiedRooms.Add(room);

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

        private void CreateLevel()
        {
            Transform currentAnchor = levelEntranceAnchor;

            for (int i = 0; i < levelRooms.Count; i++)
            {
                Vector3 translation = currentAnchor.position - levelRooms[i].entranceAnchor.position;
                levelRooms[i].gameObject.transform.position = translation;

                float angle = currentAnchor.rotation.eulerAngles.y - levelRooms[i].entranceAnchor.eulerAngles.y;
                levelRooms[i].gameObject.transform.RotateAround(currentAnchor.position, Vector3.up, angle);

                levelRooms[i].gameObject.transform.parent = this.transform;

                currentAnchor = levelRooms[i].exitAnchor;
            }
        }
    } 
}
