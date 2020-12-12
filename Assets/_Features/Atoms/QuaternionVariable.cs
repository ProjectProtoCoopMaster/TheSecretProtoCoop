using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Atom Variables/Quaternion Variable")]
    public class QuaternionVariable : ScriptableObject
    {
        public Quaternion Value;

        public void SetValue(Quaternion value) => Value = value;


    }
}