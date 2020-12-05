using UnityEngine;

namespace Gameplay.PC.Player
{
    public class PC_GrabDeadGuardsBehaviour : MonoBehaviour
    {
        [SerializeField] Transform grabOrigin = null;
        [SerializeField] Transform playerHead = null;
        SpringJoint springJoint;
        RaycastHit hitInfo;

        [SerializeField] LayerMask pickupLayer;
        [SerializeField] Rigidbody interactableObject = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                GrabObject();

            if (Input.GetKeyUp(KeyCode.F))
                ReleaseObject();
        }

        private void GrabObject()
        {
            interactableObject = GetNearestDeadGuard();

            if (interactableObject != null)
            {
                interactableObject.useGravity = false;
                interactableObject.isKinematic = true;
                interactableObject.transform.parent = grabOrigin;
                interactableObject.transform.position = grabOrigin.position;
                interactableObject.transform.rotation = grabOrigin.rotation;
            }
        }

        private void ReleaseObject()
        {
            if (interactableObject != null)
            {
                interactableObject.isKinematic = false;
                interactableObject.useGravity = true;
                interactableObject.transform.parent = null;
                interactableObject.AddForce(playerHead.transform.forward * 30, ForceMode.Impulse);
                interactableObject = null;
            }
        }

        private Rigidbody GetNearestDeadGuard()
        {
            Debug.DrawRay(playerHead.position, playerHead.forward, Color.green);
            if (Physics.Raycast(playerHead.position, playerHead.forward, out hitInfo, 500f, pickupLayer))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo.collider.CompareTag("Dead"))
                    return hitInfo.collider.GetComponent<Rigidbody>();

                else return null;
            }

            else return null;
        }
    }

}