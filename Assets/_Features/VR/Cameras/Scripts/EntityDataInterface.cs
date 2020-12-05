using UnityEngine;

namespace Gameplay.VR
{
    public class EntityDataInterface : MonoBehaviour
    {
        // need to be identical across both Detection and Overwatch
        [SerializeField] internal float rangeOfVision;
        [SerializeField] internal float coneOfVision;
    }
}