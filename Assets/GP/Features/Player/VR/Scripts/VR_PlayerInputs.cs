using System;
using UnityEngine;
using Valve.VR;

public class VR_PlayerInputs : MonoBehaviour
{
    [SerializeField] protected SteamVR_Action_Boolean inputAction;

    [SerializeField] protected SteamVR_Input_Sources handSource;

    protected SteamVR_Behaviour_Pose controllerPose;

    public virtual void Awake()
    {
        controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
        handSource = controllerPose.inputSource;
    }

    public virtual void Update() 
    {
        if(inputAction.GetStateDown(handSource)) PerformVRInputAction();
    }

    public virtual void PerformVRInputAction() { }
}