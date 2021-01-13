﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

namespace Gameplay.VR.Player
{
    public class VR_UIRaycaster : LaserPointer
    {
        [SerializeField] private SteamVR_Action_Boolean clickAction = null;
        private SteamVR_Behaviour_Pose controllerPose = null;
        private SteamVR_Input_Sources handSource;

        private MaterialPropertyBlock clickedColor;

        [SerializeField] private Button currentButton;
        [SerializeField] private Color clickColor;

        [SerializeField] private GameEvent onHover, onClick;


        new void Awake()
        {
            base.Awake();
            controllerPose = GetComponentInParent<SteamVR_Behaviour_Pose>();
            handSource = controllerPose.inputSource;

            clickedColor = new MaterialPropertyBlock();
            clickedColor.SetColor("_EmissionColor", clickColor);
        }

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
                }

                else currentButton = null;

            }
        }
    }
}