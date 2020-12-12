using System.Collections;
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
        public Dictionary<Modifiers, float> modifierSettings = new Dictionary<Modifiers, float>();

        public Pool easyPool, mediumPool, hardPool;

        public Transform levelEntranceAnchor;

        private List<RoomManager> levelRooms = new List<RoomManager>();

        void Start()
        {
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            List<Pool> pools = new List<Pool> { easyPool, mediumPool, hardPool };

            foreach (Pool pool in pools)
            {
                PickRoom(pool.rooms, pool.occurences);
            }

            MakeLevel();
        }

        private void PickRoom(List<RoomManager> rooms, int occurences)
        {
            List<RoomManager> roomPool = rooms;
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

        }
    } 
}
