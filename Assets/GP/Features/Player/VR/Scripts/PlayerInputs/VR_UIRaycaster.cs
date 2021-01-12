using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.VR.Player
{
    public class VR_UIRaycaster : MonoBehaviour
    {
        RaycastHit hitInfo;
        public LayerMask uILayers;
        public int updateFrequency;
        int framesPassed;

        public Button currentButton;
        LineRenderer lineRenderer;

        Transform hitPoint;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && currentButton != null)
                currentButton.onClick.Invoke();
        }

        private void FixedUpdate()
        {
            if (framesPassed % updateFrequency == 0)
            {
                if (Physics.Raycast(transform.position, transform.position + transform.forward * 20, out hitInfo, uILayers))
                {
                    hitPoint.position = hitInfo.point;
                    // if you hit a new button
                    if (hitInfo.collider.gameObject.GetComponent<Button>() != null && hitInfo.collider.gameObject.GetComponent<Button>() != currentButton)
                        currentButton = hitInfo.collider.gameObject.GetComponent<Button>();
                }

                else
                {
                    hitPoint.position = transform.position + transform.forward * 20;
                    currentButton = null;
                }

                framesPassed = 0;
            }

            framesPassed++;
        }

        private void LateUpdate()
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitPoint.position);
        }
    }
}