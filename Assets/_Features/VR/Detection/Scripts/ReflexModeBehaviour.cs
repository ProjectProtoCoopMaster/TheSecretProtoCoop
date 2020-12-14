using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.VR.Feedbacks
{
    public class ReflexModeBehaviour : MonoBehaviour
    {
        [SerializeField] [Tooltip("Time will slow down by x amount when the player is detected.")] [FoldoutGroup("Slow Motion")] float reflexModeMultiplier;

        public void GE_ReflexModeEngaged()
        {
            Time.timeScale /= reflexModeMultiplier;
        }

        public void GE_ReflexModeDisengage()
        {
            Time.timeScale *= reflexModeMultiplier;
        }
    }
}