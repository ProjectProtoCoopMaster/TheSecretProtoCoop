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

        Transform target;
        Transform hitPoint;

        private void Awake()
        {
            target = transform.GetChild(0);
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
                if (Physics.Raycast(transform.position, target.position - transform.position, out hitInfo, 100f, uILayers))
                {
                    // if you hit a new button
                    if (hitInfo.collider.gameObject.GetComponent<Button>() != null && hitInfo.collider.gameObject.GetComponent<Button>() != currentButton)
                        currentButton = hitInfo.collider.gameObject.GetComponent<Button>();
                    hitPoint.position = hitInfo.point;
                }
                else
                {
                    hitPoint = target;
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