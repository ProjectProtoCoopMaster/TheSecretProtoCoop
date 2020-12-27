using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.VR
{
    public class LevelEndDetection : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Events.UnityEvent _onTriggerEnter;
        private bool isEnter= false;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (!isEnter)
                {
                    _onTriggerEnter.Invoke();
                    isEnter = true;
                }

            }
        }
    }


}
