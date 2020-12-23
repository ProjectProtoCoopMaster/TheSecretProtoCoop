using UnityEngine;

public class PC_PlayerLookBehaviour : MonoBehaviour
{
    [Tooltip("A reference to the Player's Head")] 
    [SerializeField] Transform playerHead = null;
    [Tooltip("The Mouse Sensitivity. Can vary depending on the PC.")] 
    [SerializeField] float mouseSensitivity = 600f;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerHead.transform.parent.Rotate(Vector3.up, mouseX);
    }
}
