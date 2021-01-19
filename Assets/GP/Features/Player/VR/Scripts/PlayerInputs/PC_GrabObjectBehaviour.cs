using Gameplay.VR;
using UnityEngine;

namespace Gameplay.PC.Player
{
    public class PC_GrabObjectBehaviour : MonoBehaviour
    {
        [Tooltip("Key that fires the Grab input.")]
        [SerializeField] KeyCode grabKeycode = default;

        [Tooltip("Point from which the Grab is initiated.")]
        [SerializeField] Transform grabOrigin = null;
        [Tooltip("A reference to the Player's Head")]
        [SerializeField] Transform playerHead = null;
        RaycastHit hitInfo;

        [Tooltip("The Layers on which the Player can grab Objects")]
        [SerializeField] LayerMask pickupLayer;
        [Tooltip("The Current Object that is being picked up")] 
        [SerializeField] LureObjectBehaviour lureObject = null;

        private void Update()
        {
            if (Input.GetKeyDown(grabKeycode))
                GrabObject();

            if (Input.GetKeyUp(grabKeycode))
                ReleaseObject();
        }

        private void GrabObject()
        {
            lureObject = GetNearestInteractable();

            if (lureObject != null)
            {
                lureObject.rigidBody.useGravity = false;
                lureObject.rigidBody.isKinematic = true;
                lureObject.transform.parent = grabOrigin;
                lureObject.transform.position = grabOrigin.position;
                lureObject.transform.rotation = grabOrigin.rotation;
            }
        }

        private void ReleaseObject()
        {
            if (lureObject != null)
            {
                lureObject.rigidBody.isKinematic = false;
                lureObject.rigidBody.useGravity = true;
                lureObject.transform.parent = null;
                lureObject.rigidBody.AddForce(playerHead.transform.forward * 10, ForceMode.Impulse);
                lureObject = null;
            }
        }

        private LureObjectBehaviour GetNearestInteractable()
        {
            Debug.DrawRay(playerHead.position, playerHead.forward, Color.green);
            if(Physics.Raycast(playerHead.position, playerHead.forward, out hitInfo, 500f, pickupLayer))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo.collider.CompareTag("Interactable"))
                    return hitInfo.collider.GetComponent<LureObjectBehaviour>();

                else return null;
            }

            else return null;
        }
    } 
}
