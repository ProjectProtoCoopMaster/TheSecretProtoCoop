#if UNITY_STANDALONE
using Gameplay.VR.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.PC.Player
{
    public class PC_TeleportBehaviour : MonoBehaviour
    {
        [SerializeField] Transform teleportOrigin = null;
        TeleportManager teleportationManager = null;

        private void Awake()
        {
            teleportationManager = GetComponent<TeleportManager>();
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
        }
    }
}
#endif