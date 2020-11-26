using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Atom Variables/GameObject Variable")]
    public class GameObjectVariable : ScriptableObject
    {
        public GameObject Value;

        public void SetValue(GameObject value) => Value = value;
    }
}
