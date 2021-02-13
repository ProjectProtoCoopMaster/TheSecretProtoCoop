#if UNITY_STANDALONE
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

        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem impactVFXPrefab = null;


        ParticleSystem impactVFX;

        ParticleSystem _impactVFX
        {
            get
            {
                if(impactVFX == null)
                {
                    impactVFX = Instantiate(impactVFXPrefab);
                    return impactVFX;
                }
                else return impactVFX;
            }
            set
            {
                impactVFX = value;
            }
        }

        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shooting = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent ricochet = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent hitEnemy = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunReloading = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent gunReloaded = null;
        [SerializeField] [FoldoutGroup("Shooting")] AnimationClip reloadAnimation = null;
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
            if (timePassed >= 0)
            {
                timePassed -= Time.unscaledDeltaTime;
                // call the reload animation event when the gun is reloaded - the time it takes to animate the reload
                if (timePassed <= 0 + reloadAnimation.length) gunReloaded.Raise();
            }

            if (shootAction.GetStateDown(handSource))
                TryShooting();
        }

        void TryShooting()
        {
            if (timePassed <= 0)
            {
                timePassed = shootingCooldown.Value;

                shooting.Raise();

                if (Physics.SphereCast(transform.position, bulletRadius.Value, (target.position - gunBarrel.position).normalized, out hitInfo, 100f, shootingLayer))
                {
                    Debug.Log("I shot and hit " + hitInfo.collider.gameObject.name);

                    if (hitInfo.collider.CompareTag("Enemy/Light Guard"))
                    {
                        hitEnemy.Raise();
                        hitInfo.collider.GetComponentInParent<IKillable>().Die((transform.forward) * bulletForce.Value);
                    }

                    else if (hitInfo.collider.CompareTag("Enemy/Heavy Guard"))
                    {
                        hitEnemy.Raise();
                        ricochet.Raise();
                    }

                    else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                    {
                        ricochet.Raise();
                    }

                    else if (hitInfo.collider.gameObject.tag == "Jammer")
                    {
                        hitInfo.collider.GetComponent<IKillable>().Die();
                    }

                    _impactVFX.transform.position = hitInfo.point;
                    _impactVFX.transform.up = hitInfo.normal;
                    _impactVFX.Play();
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