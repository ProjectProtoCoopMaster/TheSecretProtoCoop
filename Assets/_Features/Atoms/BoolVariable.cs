using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(menuName = "Atom Variables/Bool Variable")]
    public class BoolVariable : ScriptableObject
    {
        public bool Value;

        public void SetValue(bool value) => Value = value;

        public bool isTrue()
        {
            return Value;
        }

        public void SwitchValue()
        {
            if (Value) Value = false;
            else Value = true;

            SetDirty();
        }
    }
}