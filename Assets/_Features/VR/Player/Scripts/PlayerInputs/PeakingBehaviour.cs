using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class PeakingBehaviour : MonoBehaviour
    {
        public LayerMask hidingLayerMask;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == hidingLayerMask)
                this.gameObject.layer = LayerMask.NameToLayer("Peaking");
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == hidingLayerMask)
                this.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

}