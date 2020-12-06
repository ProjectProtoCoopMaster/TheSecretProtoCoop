using Gameplay.VR;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.PC.Player
{
    public class PC_ShootBehaviour : MonoBehaviour
    {
        [SerializeField] Transform shootOrigin = null;

        [SerializeField] [FoldoutGroup("Shooting")] LayerMask shootingLayer;
        [SerializeField] [FoldoutGroup("Shooting")] float bulletRadius = 0.1f;
        [SerializeField] [FoldoutGroup("Shooting")] float bulletForce = 5f;
        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail = null;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shooting;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent ricochet;
        [SerializeField] [FoldoutGroup("Shooting")] GameEvent shotEnvironment;

        RaycastHit hitInfo;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                Shooting();
        }

        void Shooting()
        {
            shotTrail.transform.position = shootOrigin.position;
            shotTrail.transform.rotation = shootOrigin.rotation;

            shotTrail.Play();

            shooting.Raise();

            if (Physics.SphereCast(shootOrigin.position, bulletRadius, transform.forward, out hitInfo, 100f, shootingLayer))
            {
                if (hitInfo.collider.CompareTag("Enemy/Light Guard")) hitInfo.collider.GetComponentInParent<AgentDeath>().Die((transform.forward) * bulletForce);

                else if (hitInfo.collider.CompareTag("Enemy/Heavy Guard")) ricochet.Raise();

                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    ricochet.Raise();
                    shotEnvironment.Raise();
                }
            }
        }
    }
}