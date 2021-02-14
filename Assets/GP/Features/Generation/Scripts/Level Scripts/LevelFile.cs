using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 }

    [System.Serializable]
    [HideReferenceObjectPicker]
    public class PoolData
    {
        public int amountOfRoomsToPick;
        public List<RoomData> rooms = new List<RoomData>();
    }

    [System.Serializable]
    [HideReferenceObjectPicker]
    public class RoomData
    {
        public string roomName;
        public ModifierType roomModifier { get; set; } = 0;
    }

    [CreateAssetMenu(fileName = "New Level File", menuName = "Level/Level File")]
    public class LevelFile : SerializedScriptableObject
    {
        public Dictionary<Difficulty, PoolData> pools = new Dictionary<Difficulty, PoolData>();
    }
}
