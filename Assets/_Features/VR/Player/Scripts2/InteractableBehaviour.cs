using System;
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
            if (gameObject.tag != "Interactable") gameObject.tag = "Interactable";
            rigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                MakeNoise(collisionForce);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                ArmSound();
        }

        private void ArmSound()
        {
            //Debug.Log("Sound Object is armed");
        }

        private void MakeNoise(float collisionForce)
        {
            Debug.Log("I hit the target at a force of " + collisionForce);
        }
    }
}