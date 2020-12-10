using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Atom Variables/Int Variable")]
    public class IntVariable : ScriptableObject
    {
        public int Value;

        public void SetValue(int value) => Value = value;
    }
}