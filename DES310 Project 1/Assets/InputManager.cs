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
                },
                {
                    ""name"": ""Increment Ability Select"",
                    ""type"": ""Button"",
                    ""id"": ""5e168547-17f6-47aa-b17e-b86e28435a16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Decrement Ability Select"",
                    ""type"": ""Button"",
                    ""id"": ""49dfca90-caa7-4232-852d-8330b462064f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Button"",
                    ""id"": ""76a78281-05ac-4b9f-bde8-fc8cdbc9ffa2"",
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
                    ""name"": """",
                    ""id"": ""2ea1408c-9dfe-48d0-abbc-929e897bf169"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Increment Ability Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a115628c-0768-47e1-bddd-418edddd35cc"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Decrement Ability Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b49b30f-2a10-43fd-bd80-3774b1fd446e"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Keyboard
        m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
        m_Keyboard_Changetoability1 = m_Keyboard.FindAction("Change to ability 1", throwIfNotFound: true);
        m_Keyboard_Changetoability2 = m_Keyboard.FindAction("Change to ability 2", throwIfNotFound: true);
        m_Keyboard_Changetoability3 = m_Keyboard.FindAction("Change to ability 3", throwIfNotFound: true);
        m_Keyboard_Changetoability4 = m_Keyboard.FindAction("Change to  ability 4", throwIfNotFound: true);
        m_Keyboard_IncrementAbilitySelect = m_Keyboard.FindAction("Increment Ability Select", throwIfNotFound: true);
        m_Keyboard_DecrementAbilitySelect = m_Keyboard.FindAction("Decrement Ability Select", throwIfNotFound: true);
        m_Keyboard_Mouse = m_Keyboard.FindAction("Mouse", throwIfNotFound: true);
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
    private readonly InputAction m_Keyboard_Changetoability1;
    private readonly InputAction m_Keyboard_Changetoability2;
    private readonly InputAction m_Keyboard_Changetoability3;
    private readonly InputAction m_Keyboard_Changetoability4;
    private readonly InputAction m_Keyboard_IncrementAbilitySelect;
    private readonly InputAction m_Keyboard_DecrementAbilitySelect;
    private readonly InputAction m_Keyboard_Mouse;
    public struct KeyboardActions
    {
        private @InputManager m_Wrapper;
        public KeyboardActions(@InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @Changetoability1 => m_Wrapper.m_Keyboard_Changetoability1;
        public InputAction @Changetoability2 => m_Wrapper.m_Keyboard_Changetoability2;
        public InputAction @Changetoability3 => m_Wrapper.m_Keyboard_Changetoability3;
        public InputAction @Changetoability4 => m_Wrapper.m_Keyboard_Changetoability4;
        public InputAction @IncrementAbilitySelect => m_Wrapper.m_Keyboard_IncrementAbilitySelect;
        public InputAction @DecrementAbilitySelect => m_Wrapper.m_Keyboard_DecrementAbilitySelect;
        public InputAction @Mouse => m_Wrapper.m_Keyboard_Mouse;
        public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardActions instance)
        {
            if (m_Wrapper.m_KeyboardActionsCallbackInterface != null)
            {
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
                @IncrementAbilitySelect.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnIncrementAbilitySelect;
                @IncrementAbilitySelect.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnIncrementAbilitySelect;
                @IncrementAbilitySelect.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnIncrementAbilitySelect;
                @DecrementAbilitySelect.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnDecrementAbilitySelect;
                @DecrementAbilitySelect.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnDecrementAbilitySelect;
                @DecrementAbilitySelect.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnDecrementAbilitySelect;
                @Mouse.started -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_KeyboardActionsCallbackInterface.OnMouse;
            }
            m_Wrapper.m_KeyboardActionsCallbackInterface = instance;
            if (instance != null)
            {
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
                @IncrementAbilitySelect.started += instance.OnIncrementAbilitySelect;
                @IncrementAbilitySelect.performed += instance.OnIncrementAbilitySelect;
                @IncrementAbilitySelect.canceled += instance.OnIncrementAbilitySelect;
                @DecrementAbilitySelect.started += instance.OnDecrementAbilitySelect;
                @DecrementAbilitySelect.performed += instance.OnDecrementAbilitySelect;
                @DecrementAbilitySelect.canceled += instance.OnDecrementAbilitySelect;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
            }
        }
    }
    public KeyboardActions @Keyboard => new KeyboardActions(this);
    public interface IKeyboardActions
    {
        void OnChangetoability1(InputAction.CallbackContext context);
        void OnChangetoability2(InputAction.CallbackContext context);
        void OnChangetoability3(InputAction.CallbackContext context);
        void OnChangetoability4(InputAction.CallbackContext context);
        void OnIncrementAbilitySelect(InputAction.CallbackContext context);
        void OnDecrementAbilitySelect(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
    }
}
