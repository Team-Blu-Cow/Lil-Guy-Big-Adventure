// GENERATED AUTOMATICALLY FROM 'Assets/InputManager.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputManager : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputManager()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputManager"",
    ""maps"": [
        {
            ""name"": ""Keyboard"",
            ""id"": ""f767443d-4aae-4688-89c1-a05d02d72ec0"",
            ""actions"": [
                {
                    ""name"": ""Change Item"",
                    ""type"": ""Button"",
                    ""id"": ""06165784-f918-4a48-9779-8960939dab61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change to ability 1"",
                    ""type"": ""Button"",
                    ""id"": ""1ffa0537-c6b0-46ed-8c4c-31e03ca43859"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change to ability 2"",
                    ""type"": ""Button"",
                    ""id"": ""710ddbab-8e8c-4fb6-bed6-8c30ba118132"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change to ability 3"",
                    ""type"": ""Button"",
                    ""id"": ""081c1a2f-46e1-49b1-b65f-cd268eaafb42"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Change to  ability 4"",
                    ""type"": ""Button"",
                    ""id"": ""ecc89c6e-52bd-494b-9e82-8e8ee31f4e84"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""73761fd2-065a-4df4-8874-c7c9edf81a98"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change to ability 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32c440d3-8f8a-467e-a27a-92f36469c96a"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change to ability 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21393ee8-650d-4be3-8a03-29304a488fc7"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change to ability 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1e49c8b-813d-4174-9b78-0848f0e6a757"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change to  ability 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""90ccaff7-1bb2-4f20-8d8d-832a77d72657"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Item"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e4f07488-4349-4137-ac0e-5365089a47e3"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""67f28943-38bf-4283-b3f2-69465556a245"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Item"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard
        m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
        m_Keyboard_ChangeItem = m_Keyboard.FindAction("Change Item", throwIfNotFound: true);
        m_Keyboard_Changetoability1 = m_Keyboard.FindAction("Change to ability 1", throwIfNotFound: true);
        m_Keyboard_Changetoability2 = m_Keyboard.FindAction("Change to ability 2", throwIfNotFound: true);
        m_Keyboard_Changetoability3 = m_Keyboard.FindAction("Change to ability 3", throwIfNotFound: true);
        m_Keyboard_Changetoability4 = m_Keyboard.FindAction("Change to  ability 4", throwIfNotFound: true);
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

    // Keyboard
    private readonly InputActionMap m_Keyboard;
    private IKeyboardActions m_KeyboardActionsCallbackInterface;
    private readonly InputAction m_Keyboard_ChangeItem;
    private readonly InputAction m_Keyboard_Changetoability1;
    private readonly InputAction m_Keyboard_Changetoability2;
    private readonly InputAction m_Keyboard_Changetoability3;
    private readonly InputAction m_Keyboard_Changetoability4;
    public struct KeyboardActions
    {
        private @InputManager m_Wrapper;
        public KeyboardActions(@InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeItem => m_Wrapper.m_Keyboard_ChangeItem;
        public InputAction @Changetoability1 => m_Wrapper.m_Keyboard_Changetoability1;
        public InputAction @Changetoability2 => m_Wrapper.m_Keyboard_Changetoability2;
        public InputAction @Changetoability3 => m_Wrapper.m_Keyboard_Changetoability3;
        public InputAction @Changetoability4 => m_Wrapper.m_Keyboard_Changetoability4;
        public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardActions instance)
        {
            if (m_Wrapper.m_KeyboardActionsCallbackInterface != null)
            {
                @ChangeItem.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangeItem;
                @ChangeItem.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangeItem;
                @ChangeItem.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangeItem;
                @Changetoability1.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability1;
                @Changetoability1.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability1;
                @Changetoability1.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability1;
                @Changetoability2.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability2;
                @Changetoability2.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability2;
                @Changetoability2.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability2;
                @Changetoability3.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability3;
                @Changetoability3.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability3;
                @Changetoability3.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability3;
                @Changetoability4.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability4;
                @Changetoability4.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability4;
                @Changetoability4.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnChangetoability4;
            }
            m_Wrapper.m_KeyboardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeItem.started += instance.OnChangeItem;
                @ChangeItem.performed += instance.OnChangeItem;
                @ChangeItem.canceled += instance.OnChangeItem;
                @Changetoability1.started += instance.OnChangetoability1;
                @Changetoability1.performed += instance.OnChangetoability1;
                @Changetoability1.canceled += instance.OnChangetoability1;
                @Changetoability2.started += instance.OnChangetoability2;
                @Changetoability2.performed += instance.OnChangetoability2;
                @Changetoability2.canceled += instance.OnChangetoability2;
                @Changetoability3.started += instance.OnChangetoability3;
                @Changetoability3.performed += instance.OnChangetoability3;
                @Changetoability3.canceled += instance.OnChangetoability3;
                @Changetoability4.started += instance.OnChangetoability4;
                @Changetoability4.performed += instance.OnChangetoability4;
                @Changetoability4.canceled += instance.OnChangetoability4;
            }
        }
    }
    public KeyboardActions @Keyboard => new KeyboardActions(this);
    public interface IKeyboardActions
    {
        void OnChangeItem(InputAction.CallbackContext context);
        void OnChangetoability1(InputAction.CallbackContext context);
        void OnChangetoability2(InputAction.CallbackContext context);
        void OnChangetoability3(InputAction.CallbackContext context);
        void OnChangetoability4(InputAction.CallbackContext context);
    }
}
