#if UNITY_STANDALONE
using Gameplay.AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_PickupDeadGuardRay : VR_Raycaster
    {
        [SerializeField, ReadOnly] AgentManager currentGuard;

        private void Update()
        {
            if (clickAction.GetStateDown(handSource))
            {
                if (currentGuard != null)
                {
                    onClick.Raise();
                }

                laserPointer.SetPropertyBlock(clickedColor);
            }

            if (clickAction.GetStateUp(handSource))
                laserPointer.SetPropertyBlock(baseColor);
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();

            if (framesPassed % updateFrequency == 0)
            {
                // if you touch a new button
                if (hitInfo.collider != null &&
                    hitInfo.collider.gameObject.GetComponentInParent<AgentManager>() != null &&
                    hitInfo.collider.gameObject.GetComponentInParent<AgentManager>() != currentGuard)
                {
                    currentGuard = hitInfo.collider.gameObject.GetComponentInParent<AgentManager>();
                    onHover.Raise();
                    Debug.Log("Raising");
                }

                else if (hitInfo.collider == null) currentGuard = null;
            }

            showLaser = hitInfo.collider.CompareTag("Dead");
        }
    }
} 
#endif