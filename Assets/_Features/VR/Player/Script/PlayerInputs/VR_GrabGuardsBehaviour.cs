#if UNITY_STANDALONE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_GrabGuardsBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Boolean grabAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource;

        [SerializeField] LayerMask pickupLayer;
        [SerializeField] float pickupOffset = 0.2f;
        [SerializeField] Rigidbody deadGuard = null;

        FixedJoint myJoint;

        private void Awake()
        {
            controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
            myJoint = GetComponent<FixedJoint>();
            handSource = controllerPose.inputSource;
        }

        private void Update()
        {
            if (grabAction.GetStateDown(handSource))
                GrabGuard();

            if (grabAction.GetStateUp(handSource))
                ReleaseGuard();
        }

        private void GrabGuard()
        {
            deadGuard = GetNearestDeadGuard();

            if (deadGuard != null)
            {
                myJoint.connectedBody = deadGuard;
            }
        }

        private void ReleaseGuard()
        {
            if (deadGuard != null)
            {
                deadGuard.velocity = controllerPose.GetVelocity();
                deadGuard.angularVelocity = controllerPose.GetAngularVelocity();

                myJoint.connectedBody = null;
                deadGuard = null;
            }
        }

        private Rigidbody GetNearestDeadGuard()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, pickupOffset, pickupLayer);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Dead"))
                    {
                        Debug.Log("I'm picking up " + colliders[i].gameObject.name);
                        return colliders[i].transform.GetComponent<Rigidbody>();
                    }
                    else continue;
                }
                return null;
            }
            else return null;
        }
    }

} 
#endif