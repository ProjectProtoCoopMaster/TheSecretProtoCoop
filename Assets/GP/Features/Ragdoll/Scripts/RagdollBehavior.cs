using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class RagdollBehavior : MonoBehaviour
    {
        public Rigidbody[] rigidbodies;

        [Button]
        public void GetAllRigidbodies()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigidbodies.Length; i++) rigidbodies[i].isKinematic = true;
        }

        [Button]
        public void ActivateRagdoll()
        {
            for (int i = 0; i < rigidbodies.Length; i++) rigidbodies[i].isKinematic = false;
        }

        public void ActivateRagdollWithForce(Vector3 force, ForceMode mode)
        {
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                rigidbodies[i].isKinematic = false;
                rigidbodies[i].AddForce(force, mode);
            }
        }
    }
}

