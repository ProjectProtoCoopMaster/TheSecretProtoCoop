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

            if (Physics.SphereCast(shootOrigin.position, bulletRadius, transform.forward, out hitInfo, 100f, shootingLayer))
            {
                Debug.Log("I shot and hit " + hitInfo.collider.gameObject.name);

                if (hitInfo.collider.CompareTag("Enemy/Light Guard"))
                    hitInfo.collider.GetComponentInParent<AgentDeath>().Die();
            }

            else Debug.Log("I shot but didn't hit anything");
        }
    }
}