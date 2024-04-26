//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/_OurAssets/Shared/dummy/PlayerInput/KnightInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @KnightInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @KnightInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""KnightInputActions"",
    ""maps"": [
        {
            ""name"": ""Knight"",
            ""id"": ""a90de034-1b3a-456b-ac3d-a798b46e79c5"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""1ab0212f-918f-41bd-b2e3-fb9c4e3c9698"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a5e35e85-6b02-4fb7-8369-22f4cf749d9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""4071095f-d429-4822-920f-daed3e153b42"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""dabcdbf9-30d6-4ac9-8ef4-51ae3458a3b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HoldingUp"",
                    ""type"": ""Button"",
                    ""id"": ""3ea1e332-8275-4a26-83c9-2c819cfa37ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HoldingDown"",
                    ""type"": ""Button"",
                    ""id"": ""884513aa-5067-47af-a3d6-bd1ece8af6c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""8b8708ce-b8f8-47b1-a0df-3bc6496f41da"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4a52102e-a7f6-43ea-848c-f2a3cdaf77fb"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a7fe48d3-7569-4b36-a87b-a9523f7101ef"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""4f07416a-59c2-4d4a-840f-ff3fc9716a9c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6e92f2be-dee2-4d07-a695-31d689c93bb9"",
                    ""path"": ""<DualSenseGamepadHID>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b6d90eb9-f3c1-461f-901f-33865a61e013"",
                    ""path"": ""<DualSenseGamepadHID>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3ee60b9d-9d50-4f00-8bfe-8b735f38a73f"",
                    ""path"": ""<Keyboard>/numpad0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38ac68dd-5e5e-4c98-b560-637188b21204"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""768647b7-e689-42fa-aa1c-59b4c4ebebbe"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldingUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32a91036-2ac6-4e91-a818-6a2cdb630ecd"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldingDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40348bd1-9b1b-455a-8ea7-9cc227134f57"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Knight
        m_Knight = asset.FindActionMap("Knight", throwIfNotFound: true);
        m_Knight_Movement = m_Knight.FindAction("Movement", throwIfNotFound: true);
        m_Knight_Jump = m_Knight.FindAction("Jump", throwIfNotFound: true);
        m_Knight_Attack = m_Knight.FindAction("Attack", throwIfNotFound: true);
        m_Knight_Special = m_Knight.FindAction("Special", throwIfNotFound: true);
        m_Knight_HoldingUp = m_Knight.FindAction("HoldingUp", throwIfNotFound: true);
        m_Knight_HoldingDown = m_Knight.FindAction("HoldingDown", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Knight
    private readonly InputActionMap m_Knight;
    private List<IKnightActions> m_KnightActionsCallbackInterfaces = new List<IKnightActions>();
    private readonly InputAction m_Knight_Movement;
    private readonly InputAction m_Knight_Jump;
    private readonly InputAction m_Knight_Attack;
    private readonly InputAction m_Knight_Special;
    private readonly InputAction m_Knight_HoldingUp;
    private readonly InputAction m_Knight_HoldingDown;
    public struct KnightActions
    {
        private @KnightInputActions m_Wrapper;
        public KnightActions(@KnightInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Knight_Movement;
        public InputAction @Jump => m_Wrapper.m_Knight_Jump;
        public InputAction @Attack => m_Wrapper.m_Knight_Attack;
        public InputAction @Special => m_Wrapper.m_Knight_Special;
        public InputAction @HoldingUp => m_Wrapper.m_Knight_HoldingUp;
        public InputAction @HoldingDown => m_Wrapper.m_Knight_HoldingDown;
        public InputActionMap Get() { return m_Wrapper.m_Knight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KnightActions set) { return set.Get(); }
        public void AddCallbacks(IKnightActions instance)
        {
            if (instance == null || m_Wrapper.m_KnightActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_KnightActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Special.started += instance.OnSpecial;
            @Special.performed += instance.OnSpecial;
            @Special.canceled += instance.OnSpecial;
            @HoldingUp.started += instance.OnHoldingUp;
            @HoldingUp.performed += instance.OnHoldingUp;
            @HoldingUp.canceled += instance.OnHoldingUp;
            @HoldingDown.started += instance.OnHoldingDown;
            @HoldingDown.performed += instance.OnHoldingDown;
            @HoldingDown.canceled += instance.OnHoldingDown;
        }

        private void UnregisterCallbacks(IKnightActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Special.started -= instance.OnSpecial;
            @Special.performed -= instance.OnSpecial;
            @Special.canceled -= instance.OnSpecial;
            @HoldingUp.started -= instance.OnHoldingUp;
            @HoldingUp.performed -= instance.OnHoldingUp;
            @HoldingUp.canceled -= instance.OnHoldingUp;
            @HoldingDown.started -= instance.OnHoldingDown;
            @HoldingDown.performed -= instance.OnHoldingDown;
            @HoldingDown.canceled -= instance.OnHoldingDown;
        }

        public void RemoveCallbacks(IKnightActions instance)
        {
            if (m_Wrapper.m_KnightActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IKnightActions instance)
        {
            foreach (var item in m_Wrapper.m_KnightActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_KnightActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public KnightActions @Knight => new KnightActions(this);
    public interface IKnightActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
        void OnHoldingUp(InputAction.CallbackContext context);
        void OnHoldingDown(InputAction.CallbackContext context);
    }
}
