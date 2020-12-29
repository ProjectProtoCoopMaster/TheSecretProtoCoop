using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Atom Variables/Vector3 Variable")]
    public class Vector3Variable : ScriptableObject
    {
        public Vector3 Value;

        public void SetValue(Vector3 value) => Value = value;


    }
}
