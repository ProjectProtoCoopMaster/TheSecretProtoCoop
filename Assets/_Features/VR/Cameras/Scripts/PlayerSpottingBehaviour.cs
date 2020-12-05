using UnityEngine;

namespace Gameplay.VR
{
    public class PlayerSpottingBehaviour : MonoBehaviour
    {
        DetectionBehavior detectionBehavior = null;

        private void Awake()
        {
            detectionBehavior = GetComponent<DetectionBehavior>();
        }

        private void Update()
        {
          //  detectionBehavior.sqrDistToTarget;
        }
    } 
}
