using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Level variable", menuName = "Level/Level Variable")]
    public class LevelVariable : ScriptableObject
    {
        public List<RoomData> pickedRooms { get; set; } = new List<RoomData>();
    } 
}
