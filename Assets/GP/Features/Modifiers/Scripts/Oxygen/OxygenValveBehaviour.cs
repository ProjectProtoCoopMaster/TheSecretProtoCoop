using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR
{
    public class OxygenValveBehaviour : MonoBehaviour
    {
        [ReadOnly] [ShowInInspector] int currentValue, lastValue;

        [ReadOnly] [ShowInInspector] int tickRate = 120;
        [ReadOnly] [ShowInInspector] int framesPassed = 0;

        private void FixedUpdate()
        {
            framesPassed++;

            if (framesPassed % tickRate == 0)
            {
                lastValue = currentValue;
                currentValue = (int)transform.rotation.eulerAngles.x;

                if (currentValue < lastValue) Debug.Log("Full turn");

                framesPassed = 0;
            }
        }
    }
}
