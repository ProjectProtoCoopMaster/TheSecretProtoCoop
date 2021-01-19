using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Atom Variables/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        public float Value;

        public void SetValue(float value) => Value = value;
    }
}