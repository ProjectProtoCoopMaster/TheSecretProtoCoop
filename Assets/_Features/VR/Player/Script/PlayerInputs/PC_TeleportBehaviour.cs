#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class PC_TeleportBehaviour : MonoBehaviour
    {
        Transform playerHead;
        public Transform mouseOrigin;
        [SerializeField] TeleportManager teleportationManager;
        [SerializeField] float mouseSensitivity;
        float xRotation = 0f;

        private void Start()
        {
            playerHead = teleportationManager.playerHead;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                teleportationManager.pointerOrigin = mouseOrigin;
                teleportationManager.TallRayPointer(null);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                teleportationManager.TryTeleporting();
            }

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerHead.transform.parent.Rotate(Vector3.up, mouseX);
        }
    }
} 
#endif