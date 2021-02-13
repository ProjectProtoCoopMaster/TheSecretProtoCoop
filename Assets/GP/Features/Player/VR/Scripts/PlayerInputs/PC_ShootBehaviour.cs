using Gameplay.VR;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.PC.Player
{
    public class PC_ShootBehaviour : MonoBehaviour
    {
        [Tooltip("The Key that fires the Shoot input.")] 
        [SerializeField] KeyCode shootKeycode = default;

        [Tooltip("The point from which the player's bullets will shoot.")] 
        [SerializeField] Transform shootOrigin = null;

        [Tooltip("The Particle System to play when the player shoots.")] 
        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail = null;
        [Tooltip("A GameEvent that fires when the player shoots.")] 
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shooting = null;
        [Tooltip("A GameEvent that fires when the player's bullet ricochets off an armoured guard.")]
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent ricochet = null;
        [Tooltip("A GameEvent that fires when the player's bullet hits the environement.")]
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shotEnvironment = null;
        [Tooltip("A GameEvent that fires when the player reloads his weapon.")] 
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunReloading = null;
        [Tooltip("A GameEvent that fires when the player's gun is empty.")] 
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunEmpty = null;
        [Tooltip("A reference to the PC Player's crosshair. This is where your bullets will go.")] 
        [SerializeField] [FoldoutGroup("Shooting")] Transform crossHair;

        [Tooltip("A reference to the Atom that defines the gun's Cooldown value. A higher value means less murdering.")] 
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable shootingCooldown;
        float timePassed = 2f;
        [Tooltip("A reference to the Atom that defines the radius of bullets. A higher value means it is easier to hit things (this also applies to environments).")] 
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable bulletRadius;
        [Tooltip("A reference to the Atom that defines how hard bullets will hit enemy Guards. A higher value means more force applied to the guard's ragdoll.")] 
        [SerializeField] [FoldoutGroup("Internal Values")] FloatVariable bulletForce;
        [Tooltip("")] 
        [SerializeField] [FoldoutGroup("Internal Values")] LayerMask shootingLayer = default;
        RaycastHit hitInfo = default;

        private void Update()
        {
            if (timePassed > 0)
            {
                timePassed -= Time.unscaledDeltaTime;
            }

            if (Input.GetKeyDown(shootKeycode) || Input.GetMouseButtonDown(0))
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
                    if (hitInfo.collider.CompareTag("Enemy/Light Guard")) hitInfo.collider.GetComponentInParent<IKillable>().Die((transform.forward) * bulletForce.Value);

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