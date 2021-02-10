using UnityEngine;

namespace Gameplay.VR
{
    public class VisionData : MonoBehaviour
    {
        // need to be identical across both Detection and Overwatch
        [SerializeField] public float rangeOfVision;
        [SerializeField] public float coneOfVision;

        [SerializeField] public GameEvent playerPeeking = null;
    }
}