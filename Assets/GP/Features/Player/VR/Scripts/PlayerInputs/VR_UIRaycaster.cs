#if UNITY_STANDALONE
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.VR.Player
{
    public class VR_UIRaycaster : VR_Raycaster
    {
        [SerializeField] private Button currentButton;

        private void Update()
        {
            if (clickAction.GetStateDown(handSource))
            {
                if (currentButton != null)
                {
                    currentButton.onClick.Invoke();
                    onClick.Raise();
                }

                laserPointer.SetPropertyBlock(clickedColor);
            }

            if (clickAction.GetStateUp(handSource))
                laserPointer.SetPropertyBlock(baseColor);
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();

            if (framesPassed % updateFrequency == 0)
            {
                // if you touch a new button
                if (hitInfo.collider != null &&
                    hitInfo.collider.gameObject.GetComponent<Button>() != null &&
                    hitInfo.collider.gameObject.GetComponent<Button>() != currentButton)
                {
                    currentButton = hitInfo.collider.gameObject.GetComponent<Button>();
                    onHover.Raise();
                    Debug.Log("Raising");
                }

                else if (hitInfo.collider == null) currentButton = null;
            }
        }
    }
}

#endif