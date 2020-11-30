using System;
using UnityEngine;

namespace Gameplay.VR
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]

    public class InteractableBehaviour : MonoBehaviour
    {
        // accessed by the Grab Behaviour
        internal Rigidbody rigidBody;
        AudioSource audioSource;
        private bool canPlaySound;

        private void Awake()
        {
            if (gameObject.tag != "Interactable") gameObject.tag = "Interactable";
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment") && canPlaySound)
                MakeNoise(collision.relativeVelocity.magnitude);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                ArmSound();
        }

        private void ArmSound()
        {
            canPlaySound = true;
        }

        private void MakeNoise(float collisionForce)
        {
            Debug.Log("I hit the target at a force of " + collisionForce);
            audioSource.volume = collisionForce/20f;
            audioSource.Play();

            canPlaySound = false;
        }
    }
}