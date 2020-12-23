using UnityEngine;

namespace Gameplay.PC.Player
{
    public class PC_GrabGuardsBehaviour : MonoBehaviour
    {
        [SerializeField] Transform grabOrigin = null;
        [SerializeField] Transform playerHead = null;
        RaycastHit hitInfo;

        [SerializeField] LayerMask pickupLayer;
        [SerializeField] Rigidbody deadGuard = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                GrabObject();

            if (Input.GetKeyUp(KeyCode.F))
                ReleaseObject();
        }

        private void GrabObject()
        {
            deadGuard = GetNearestDeadGuard();

            if (deadGuard != null)
            {
                deadGuard.useGravity = false;
                deadGuard.isKinematic = true;
                deadGuard.transform.parent = grabOrigin;
                deadGuard.transform.position = grabOrigin.position;
                deadGuard.transform.rotation = grabOrigin.rotation;
            }
        }

        private void ReleaseObject()
        {
            if (deadGuard != null)
            {
                deadGuard.isKinematic = false;
                deadGuard.useGravity = true;
                deadGuard.transform.parent = null;
                deadGuard.AddForce(playerHead.transform.forward * 30, ForceMode.Impulse);
                deadGuard = null;
            }
        }

        private Rigidbody GetNearestDeadGuard()
        {
            Debug.DrawRay(playerHead.position, playerHead.forward, Color.green);
            if (Physics.Raycast(playerHead.position, playerHead.forward, out hitInfo, 10f, pickupLayer))
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