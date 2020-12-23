// GENERATED AUTOMATICALLY FROM 'Assets/_Features/VR/Player/Input/PlayerMapping.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerMapping : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerMapping()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerMapping"",
    ""maps"": [
        {
            ""name"": ""PC_Controls"",
            ""id"": ""5bedc1eb-976f-46cf-a492-4a1213f7e57e"",
            ""actions"": [
                {
                    ""name"": ""Teleport"",
                    ""type"": ""Button"",
                    ""id"": ""f25b2d13-1d90-4c43-9a4f-64e2b8229bdd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grab"",
                    ""type"": ""Button"",
                    ""id"": ""bc5932fb-5c4b-4bb4-a3d4-ad45778080c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""777c07d9-3bcd-4b17-95d2-29a5e232d71a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""35dd7756-320e-495f-b76f-53e61b4e0368"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Teleport"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b8dd83b-3dd3-4530-84c8-a4f9efa2e8d1"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7cd421ee-6a9e-45a3-9a57-12bd990a0430"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PC_Controls
        m_PC_Controls = asset.FindActionMap("PC_Controls", throwIfNotFound: true);
        m_PC_Controls_Teleport = m_PC_Controls.FindAction("Teleport", throwIfNotFound: true);
        m_PC_Controls_Grab = m_PC_Controls.FindAction("Grab", throwIfNotFound: true);
        m_PC_Controls_Shoot = m_PC_Controls.FindAction("Shoot", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PC_Controls
    private readonly InputActionMap m_PC_Controls;
    private IPC_ControlsActions m_PC_ControlsActionsCallbackInterface;
    private readonly InputAction m_PC_Controls_Teleport;
    private readonly InputAction m_PC_Controls_Grab;
    private readonly InputAction m_PC_Controls_Shoot;
    public struct PC_ControlsActions
    {
        private @PlayerMapping m_Wrapper;
        public PC_ControlsActions(@PlayerMapping wrapper) { m_Wrapper = wrapper; }
        public InputAction @Teleport => m_Wrapper.m_PC_Controls_Teleport;
        public InputAction @Grab => m_Wrapper.m_PC_Controls_Grab;
        public InputAction @Shoot => m_Wrapper.m_PC_Controls_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_PC_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PC_ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPC_ControlsActions instance)
        {
            if (m_Wrapper.m_PC_ControlsActionsCallbackInterface != null)
            {
                @Teleport.started -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnTeleport;
                @Teleport.performed -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnTeleport;
                @Teleport.canceled -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnTeleport;
                @Grab.started -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnGrab;
                @Grab.performed -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnGrab;
                @Grab.canceled -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnGrab;
                @Shoot.started -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PC_ControlsActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_PC_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Teleport.started += instance.OnTeleport;
                @Teleport.performed += instance.OnTeleport;
                @Teleport.canceled += instance.OnTeleport;
                @Grab.started += instance.OnGrab;
                @Grab.performed += instance.OnGrab;
                @Grab.canceled += instance.OnGrab;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public PC_ControlsActions @PC_Controls => new PC_ControlsActions(this);
    public interface IPC_ControlsActions
    {
        void OnTeleport(InputAction.CallbackContext context);
        void OnGrab(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
