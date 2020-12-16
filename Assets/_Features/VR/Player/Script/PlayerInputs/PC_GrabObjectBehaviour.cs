using Gameplay.VR;
using UnityEngine;

namespace Gameplay.PC.Player
{
    public class PC_GrabObjectBehaviour : MonoBehaviour
    {
        [SerializeField] Transform grabOrigin = null;
        [SerializeField] Transform playerHead = null;
        RaycastHit hitInfo;

        [SerializeField] LayerMask pickupLayer;
        [SerializeField] InteractableBehaviour interactableObject = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                GrabObject();

            if (Input.GetKeyUp(KeyCode.F))
                ReleaseObject();
        }

        private void GrabObject()
        {
            interactableObject = GetNearestInteractable();

            if (interactableObject != null)
            {
                interactableObject.rigidBody.useGravity = false;
                interactableObject.rigidBody.isKinematic = true;
                interactableObject.transform.parent = grabOrigin;
                interactableObject.transform.position = grabOrigin.position;
                interactableObject.transform.rotation = grabOrigin.rotation;
            }
        }

        private void ReleaseObject()
        {
            if (interactableObject != null)
            {
                interactableObject.rigidBody.isKinematic = false;
                interactableObject.rigidBody.useGravity = true;
                interactableObject.transform.parent = null;
                interactableObject.rigidBody.AddForce(playerHead.transform.forward * 10, ForceMode.Impulse);
                interactableObject = null;
            }
        }

        private InteractableBehaviour GetNearestInteractable()
        {
            Debug.DrawRay(playerHead.position, playerHead.forward, Color.green);
            if(Physics.Raycast(playerHead.position, playerHead.forward, out hitInfo, 500f, pickupLayer))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo.collider.CompareTag("Interactable"))
                    return hitInfo.collider.GetComponent<InteractableBehaviour>();

                else return null;
            }

            else return null;
        }
    } 
}
