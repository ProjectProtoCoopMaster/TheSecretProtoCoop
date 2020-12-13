using UnityEngine;

namespace Gameplay.VR
{
    public class EntityDataInterface : MonoBehaviour
    {
        // need to be identical across both Detection and Overwatch
        [SerializeField] public float rangeOfVision;
        [SerializeField] public float coneOfVision;

        [SerializeField] public GameObjectVariable awarenessManagerObj = null;
        [SerializeField] public GameEvent raiseAlarm = null;

    }
}