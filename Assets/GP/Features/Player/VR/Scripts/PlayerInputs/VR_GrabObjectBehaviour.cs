#if UNITY_STANDALONE
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_GrabObjectBehaviour : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Boolean grabAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource;

        [SerializeField] LayerMask pickupLayer;
        [SerializeField] float pickupOffset = 0.2f;
        [SerializeField] LureObjectBehaviour interactableObject = null;

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
                GrabObject();

            if (grabAction.GetStateUp(handSource)) 
                ReleaseObject();
        }

        private void GrabObject()
        {
            interactableObject = GetNearestInteractable();

            if (interactableObject != null)
            {
                myJoint.connectedBody = interactableObject.rigidBody;
            }
        }

        private void ReleaseObject()
        {
            if (interactableObject != null)
            {
                interactableObject.rigidBody.velocity = controllerPose.GetVelocity();
                interactableObject.rigidBody.angularVelocity = controllerPose.GetAngularVelocity();

                myJoint.connectedBody = null;
                interactableObject = null;
            }
        }

        LureObjectBehaviour GetNearestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, pickupOffset, pickupLayer);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Interactable"))
                    {
                        Debug.Log("I'm picking up " + colliders[i].gameObject.name);
                        return colliders[i].transform.GetComponent<LureObjectBehaviour>();
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