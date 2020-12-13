﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay
{
    public enum Modifiers { DarkZone, Thermic, Oxygen, None }

    [System.Serializable]
    public class Pool
    {
        public int occurences;
        public List<RoomManager> rooms = new List<RoomManager>();
    }

    public class LevelManager : SerializedMonoBehaviour
    {
        private List<Modifiers> modifiers = new List<Modifiers>();
        public int modifierAmount;

        public Pool easyPool, mediumPool, hardPool;

        public Transform levelEntranceAnchor;

        private List<RoomManager> levelRooms = new List<RoomManager>();

        void Start()
        {
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            modifiers = new List<Modifiers> { Modifiers.DarkZone, Modifiers.Thermic, Modifiers.Oxygen };

            List<Pool> pools = new List<Pool> { easyPool, mediumPool, hardPool };

            foreach (Pool pool in pools)
            {
                PickRoom(pool.rooms, pool.occurences);
            }

            MakeLevel();
        }

        private void PickRoom(List<RoomManager> rooms, int occurences)
        {
            List<RoomManager> roomPool = new List<RoomManager>();
            foreach (RoomManager room in rooms) roomPool.Add(room);

            int pick;

            for (int i = 0; i < occurences; i++)
            {
                pick = Random.Range(0, roomPool.Count);

                levelRooms.Add(roomPool[pick]);

                roomPool.RemoveAt(pick);
            }
        }

        private void MakeLevel()
        {
            int randomModifierAmount = Random.Range(0, modifierAmount) + 1;
            int[] randomRooms = new int[randomModifierAmount];

            List<RoomManager> availableRooms = new List<RoomManager>();
            foreach (RoomManager room in levelRooms) availableRooms.Add(room);

            for (int u = 0; u < randomRooms.Length; u++)
            {
                randomRooms[u] = Random.Range(0, availableRooms.Count);

                SetRoomModifier(availableRooms[randomRooms[u]]);

                availableRooms.RemoveAt(randomRooms[u]);
            }
            availableRooms.Clear();

            ///

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
        
        private void SetRoomModifier(RoomManager room)
        {
            int m = Random.Range(0, modifiers.Count);
            Modifiers modifier = modifiers[m];

            room.roomModifier = modifier;

            modifiers.RemoveAt(m);
        }
    } 
}
