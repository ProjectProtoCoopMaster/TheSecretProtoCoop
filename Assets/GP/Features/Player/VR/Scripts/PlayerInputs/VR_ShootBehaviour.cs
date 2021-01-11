#if UNITY_STANDALONE
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_ShootBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Boolean shootAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource = default;

        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shooting = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent ricochet = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shotEnvironment = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunReloading = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunEmpty = null;

        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable shootingCooldown;
        float timePassed = 2f;
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable bulletRadius;
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable bulletForce;
        [SerializeField] [FoldoutGroup("Internal Values")] Transform gunBarrel, target;
        [SerializeField] [FoldoutGroup("Internal Values")] LayerMask shootingLayer = default;
        RaycastHit hitInfo = default;

        private void Awake()
        {
            controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;
        }

        private void Update()
        {
            if (timePassed > 0)
            {
                timePassed -= Time.unscaledDeltaTime;
            }

            if (shootAction.GetStateDown(handSource))
                TryShooting();
        }

        void TryShooting()
        {
            if (timePassed <= 0)
            {
                timePassed = shootingCooldown.Value;

                shotTrail.transform.position = gunBarrel.position;
                shotTrail.transform.LookAt(target);

                shotTrail.Play();

                shooting.Raise();

                if (Physics.SphereCast(transform.position, bulletRadius.Value, (target.position - gunBarrel.position).normalized, out hitInfo, 100f, shootingLayer))
                {
                    Debug.Log("I shot and hit " + hitInfo.collider.gameObject.name);

                    if (hitInfo.collider.CompareTag("Enemy/Light Guard"))
                    {
                        hitInfo.collider.GetComponentInParent<AgentDeath>().Die((transform.forward) * bulletForce.Value);
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

                    else if (hitInfo.collider.gameObject.tag == "Jammer")
                    {
                        hitInfo.collider.GetComponent<IKillable>().Die();
                    }
                }

                gunReloading.Raise();
            }

            else
            {
                gunEmpty.Raise();
            }
        }
    }
}
#endif