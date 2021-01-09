using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public enum Difficulty { Easy, Medium, Hard }

    [CreateAssetMenu(fileName = "New Level Pool", menuName = "Pool/Level Pool")]
    public class LevelPool : SerializedScriptableObject
    {
        public Dictionary<int, Difficulty> rooms = new Dictionary<int, Difficulty>();
    } 
}
