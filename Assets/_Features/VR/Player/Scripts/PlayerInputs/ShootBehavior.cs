#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class ShootBehavior : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("Shooting")] LayerMask shootingLayer;
        [SerializeField] [FoldoutGroup("Shooting")] ParticleSystem shotTrail;
        [SerializeField] [FoldoutGroup("SteamVR Components")] Transform controllerPosition;

        RaycastHit hit;

        // a reference to the action
        [SerializeField] [FoldoutGroup("SteamVR Components")] SteamVR_Action_Boolean shootAction;
        // a reference to the hand
        [SerializeField] [FoldoutGroup("SteamVR Components")] SteamVR_Input_Sources handType;

        private void Awake()
        {
            controllerPosition = transform;
            shootAction.AddOnStateUpListener(TriggerRelease, handType);
        }

        private void TriggerRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (shotTrail == null) 
                shotTrail = GameObject.Find("BulletTrail").GetComponent<ParticleSystem>();
            if (controllerPosition == null)
                controllerPosition = transform;

            if (shotTrail && controllerPosition)
            {
                shotTrail.transform.position = controllerPosition.position;
                shotTrail.transform.rotation = controllerPosition.rotation;

                shotTrail.Play();
                Debug.Log("Shooting");

                if (Physics.SphereCast(controllerPosition.position, 0.25f, controllerPosition.forward, out hit, 100f, shootingLayer))
                {
                    Debug.Log("Bullet FUCKING shot " + hit.collider.name);
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<GuardBehaviour>().Shot();
                        Debug.Log("Bullet MOTHAFUCKIN struck " + hit.collider.name);
                    }

                    else Debug.Log("Bullet missed and hit " + hit.collider.name);
                }
            }
            else Debug.Log("Can't find shotTrail or Controller");
        }
    }
} 
#endif