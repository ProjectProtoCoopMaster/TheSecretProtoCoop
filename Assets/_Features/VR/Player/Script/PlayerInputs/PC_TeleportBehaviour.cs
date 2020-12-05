﻿#if UNITY_STANDALONE
using Gameplay.VR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.PC.Player
{
    public class PC_TeleportBehaviour : MonoBehaviour
    {
        [SerializeField] Transform teleportOrigin = null;
        TeleportManager teleportationManager = null;
        Transform playerHead = null;
        float mouseSensitivity = 200f;
        float xRotation = 0f;

        private bool pressed;
        private bool released;

        private void Awake()
        {
            teleportationManager = GetComponent<TeleportManager>();
        }

        private void Start()
        {
            playerHead = teleportationManager.playerHead;
            Cursor.lockState = CursorLockMode.Locked;

        }
        private void OnEnable()
        {
            /*mmep.PC_Controls.Teleport.performed += ctx => pressed = true;
            mmep.PC_Controls.Teleport.canceled += ctx => released = true;
            teleportAction.performed += ctx => pressed = true;
            teleportAction.canceled += ctx => released = true;*/
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                teleportationManager.pointerOrigin = teleportOrigin;
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


            pressed = false;
            released = false;
        }
    }
}
#endif