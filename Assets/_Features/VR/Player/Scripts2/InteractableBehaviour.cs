using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(Rigidbody))]
    public class InteractableBehaviour : MonoBehaviour
    {
        // accessed by the Grab Behaviour
        internal Rigidbody rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }
}