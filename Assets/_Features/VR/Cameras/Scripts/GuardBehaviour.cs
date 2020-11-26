using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class GuardBehaviour : MonoBehaviour, IKillable
    {
        [SerializeField] GameEvent killed;
        RagdollBehavior ragdoll;

        private void Awake()
        {
            ragdoll = GetComponent<RagdollBehavior>();
        }

        // called when the player gun raycast hits the guard
        public void Shot()
        {
            gameObject.name = "DEAD";
            GE_Die();
        }

        public void GE_Die()
        {
            Debug.Log("Ragdolling");
            ragdoll.ActivateRagdoll();
        }
    }
}
