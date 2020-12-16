using Gameplay.VR;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.PC.Player
{
    public class PC_ShootBehaviour : MonoBehaviour
    {
        [SerializeField] Transform shootOrigin = null;

        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shooting = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent ricochet = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shotEnvironment = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunReloading = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunEmpty = null;
        [SerializeField] [FoldoutGroup("Shooting")] Transform crossHair;

        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable shootingCooldown;
        float timePassed = 2f;
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable bulletRadius;
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable bulletForce;
        [SerializeField] [FoldoutGroup("Internal Values")] LayerMask shootingLayer = default;
        RaycastHit hitInfo = default;

        private void Update()
        {
            if (timePassed > 0)
            {
                timePassed -= Time.unscaledDeltaTime;
            }

            if (Input.GetKeyDown(KeyCode.E))
                TryShooting();
        }

        void TryShooting()
        {
            if (timePassed <= 0)
            {
                timePassed = shootingCooldown.Value;

                shotTrail.transform.position = shootOrigin.position;
                shotTrail.transform.LookAt(crossHair.position);

                shotTrail.Play();

                shooting.Raise();

                if (Physics.SphereCast(shootOrigin.position, bulletRadius.Value, (crossHair.position - shootOrigin.position).normalized, out hitInfo, 100f, shootingLayer))
                {
                    if (hitInfo.collider.CompareTag("Enemy/Light Guard")) hitInfo.collider.GetComponentInParent<AgentDeath>().Die((transform.forward) * bulletForce.Value);

                    else if (hitInfo.collider.CompareTag("Enemy/Heavy Guard")) ricochet.Raise();

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