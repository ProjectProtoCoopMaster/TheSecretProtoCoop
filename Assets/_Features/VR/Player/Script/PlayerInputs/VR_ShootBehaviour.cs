#if UNITY_STANDALONE
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_ShootBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Boolean shootAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource;

        [SerializeField] [FoldoutGroup("Shooting")] LayerMask shootingLayer;
        [SerializeField] [FoldoutGroup("Shooting")] float bulletRadius = 0.1f;
        [SerializeField] [FoldoutGroup("Shooting")] float bulletForce = 5f;
        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shooting;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent ricochet;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shotEnvironment;

        RaycastHit hitInfo;

        private void Awake()
        {
            controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;
        }

        private void Update()
        {
            if (shootAction.GetStateDown(handSource)) 
                Shooting();
        }

        void Shooting()
        {
            shotTrail.transform.position = transform.position;
            shotTrail.transform.rotation = transform.rotation;

            shotTrail.Play();

            shooting.Raise();

            if (Physics.SphereCast(transform.position, bulletRadius, transform.forward, out hitInfo, 100f, shootingLayer))
            {
                Debug.Log("I shot and hit " + hitInfo.collider.gameObject.name);

                if (hitInfo.collider.CompareTag("Enemy/Light Guard"))
                {
                    hitInfo.collider.GetComponentInParent<AgentDeath>().Die((transform.forward) * bulletForce);
                }

                else if (hitInfo.collider.CompareTag("Enemy/Heavy Guard"))
                {
                    ricochet.Raise();
                }

                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    ricochet.Raise();
                    shotEnvironment.Raise();
                }
            }

            else Debug.Log("I shot but didn't hit anything");
        }
    }
}
#endif