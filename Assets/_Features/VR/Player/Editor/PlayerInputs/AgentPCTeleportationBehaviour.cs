#if UNITY_STANDALONE
using UnityEngine;

namespace Gameplay.VR.Player
{
    public class AgentPCTeleportationBehaviour : MonoBehaviour
    {
        Transform playerHead;
        public Transform mouseOrigin;
        AgentVRTeleportationManager manager;
        [SerializeField] float mouseSensitivity;
        float xRotation = 0f;

        private void Awake()
        {
            manager = GetComponent<AgentVRTeleportationManager>();
            playerHead = manager.playerHead;
        }

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                manager.pointerOrigin = mouseOrigin;
                manager.TallRayPointer(null);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                manager.TryTeleporting();
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