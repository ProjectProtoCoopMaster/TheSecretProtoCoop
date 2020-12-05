using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.VR.Player
{
    public class PC_ShootBehaviour : MonoBehaviour
    {
        [SerializeField] InputAction shootAction = null;
        [SerializeField] Transform controllerPose = null;

        [SerializeField] [FoldoutGroup("Shooting")] LayerMask shootingLayer;
        [SerializeField] [FoldoutGroup("Shooting")] float bulletRadius = 0.1f;
        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail = null;

        RaycastHit hitInfo;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                Shooting();
        }

        void Shooting()
        {
            shotTrail.transform.position = transform.position;
            shotTrail.transform.rotation = transform.rotation;

            shotTrail.Play();

            if (Physics.SphereCast(controllerPose.position, bulletRadius, transform.forward, out hitInfo, 100f, shootingLayer))
            {
                Debug.Log("I shot and hit " + hitInfo.collider.gameObject.name);

                if (hitInfo.collider.CompareTag("Enemy/Light Guard"))
                    hitInfo.collider.GetComponentInParent<AgentDeath>().Die();
            }

            else Debug.Log("I shot but didn't hit anything");
        }
    }
}