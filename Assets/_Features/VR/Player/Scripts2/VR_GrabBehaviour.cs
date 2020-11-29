#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using UnityEngine;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_GrabBehaviour : MonoBehaviour
    {
        [SerializeField] [FoldoutGroup("SteamVR Components")] SteamVR_Action_Boolean grabAction = null;
        SteamVR_Behaviour_Pose controllerPose = null;
        SteamVR_Input_Sources handSource;

        [SerializeField] LayerMask pickupLayer;
        [SerializeField] InteractableBehaviour interactableObject = null;

        FixedJoint myJoint;

        private void Awake()
        {
            controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;
        }

        private void OnEnable()
        {
            grabAction.AddOnStateDownListener(GrabObject, handSource);
            grabAction.AddOnStateUpListener(ReleaseObject, handSource);
        }

        private void GrabObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            interactableObject = GetNearestInteractable();

            if (interactableObject != null)
            {
                interactableObject.transform.position = transform.position;
                myJoint.connectedBody = interactableObject.rigidBody;
            }
        }

        private void ReleaseObject(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            if (interactableObject != null)
            {
                interactableObject.rigidBody.velocity = controllerPose.GetVelocity();
                interactableObject.rigidBody.angularVelocity = controllerPose.GetAngularVelocity();
                myJoint.connectedBody = null;
                interactableObject = null;
            }
        }

        private InteractableBehaviour GetNearestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 0.2f, pickupLayer);

            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Interactable"))
                        return colliders[i].transform.GetComponent<InteractableBehaviour>();
                    else continue;
                }
                return null;
            }

            else return null;
        }
    }
}

#endif