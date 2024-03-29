// GENERATED AUTOMATICALLY FROM 'Assets/Input/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""PathfinderTestControls"",
            ""id"": ""004a3e80-88d7-4b04-b67c-e383ab24a816"",
            ""actions"": [
                {
                    ""name"": ""MousePos"",
                    ""type"": ""Value"",
                    ""id"": ""d5b24880-0281-46aa-8ffb-d6b570df6d79"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightMouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""7a4a121a-5cf1-4252-b9d0-f513c673660a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftMouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""8666dbd6-5207-4179-b8c3-984348d3a203"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9a8e11ed-52f1-4ef2-b21b-e063668d4e72"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard + mouse"",
                    ""action"": ""MousePos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79850271-29ef-427f-9dbe-b3da434e3867"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightMouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e1fa2f4-9121-4d51-8319-48efc10443cf"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""keyboard + mouse"",
                    ""action"": ""LeftMouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""keyboard + mouse"",
            ""bindingGroup"": ""keyboard + mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PathfinderTestControls
        m_PathfinderTestControls = asset.FindActionMap("PathfinderTestControls", throwIfNotFound: true);
        m_PathfinderTestControls_MousePos = m_PathfinderTestControls.FindAction("MousePos", throwIfNotFound: true);
        m_PathfinderTestControls_RightMouseClick = m_PathfinderTestControls.FindAction("RightMouseClick", throwIfNotFound: true);
        m_PathfinderTestControls_LeftMouseClick = m_PathfinderTestControls.FindAction("LeftMouseClick", throwIfNotFound: true);
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

    // PathfinderTestControls
    private readonly InputActionMap m_PathfinderTestControls;
    private IPathfinderTestControlsActions m_PathfinderTestControlsActionsCallbackInterface;
    private readonly InputAction m_PathfinderTestControls_MousePos;
    private readonly InputAction m_PathfinderTestControls_RightMouseClick;
    private readonly InputAction m_PathfinderTestControls_LeftMouseClick;
    public struct PathfinderTestControlsActions
    {
        private @InputMaster m_Wrapper;
        public PathfinderTestControlsActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePos => m_Wrapper.m_PathfinderTestControls_MousePos;
        public InputAction @RightMouseClick => m_Wrapper.m_PathfinderTestControls_RightMouseClick;
        public InputAction @LeftMouseClick => m_Wrapper.m_PathfinderTestControls_LeftMouseClick;
        public InputActionMap Get() { return m_Wrapper.m_PathfinderTestControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PathfinderTestControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPathfinderTestControlsActions instance)
        {
            if (m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface != null)
            {
                @MousePos.started -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnMousePos;
                @MousePos.performed -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnMousePos;
                @MousePos.canceled -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnMousePos;
                @RightMouseClick.started -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnRightMouseClick;
                @RightMouseClick.performed -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnRightMouseClick;
                @RightMouseClick.canceled -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnRightMouseClick;
                @LeftMouseClick.started -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnLeftMouseClick;
                @LeftMouseClick.performed -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnLeftMouseClick;
                @LeftMouseClick.canceled -= m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface.OnLeftMouseClick;
            }
            m_Wrapper.m_PathfinderTestControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePos.started += instance.OnMousePos;
                @MousePos.performed += instance.OnMousePos;
                @MousePos.canceled += instance.OnMousePos;
                @RightMouseClick.started += instance.OnRightMouseClick;
                @RightMouseClick.performed += instance.OnRightMouseClick;
                @RightMouseClick.canceled += instance.OnRightMouseClick;
                @LeftMouseClick.started += instance.OnLeftMouseClick;
                @LeftMouseClick.performed += instance.OnLeftMouseClick;
                @LeftMouseClick.canceled += instance.OnLeftMouseClick;
            }
        }
    }
    public PathfinderTestControlsActions @PathfinderTestControls => new PathfinderTestControlsActions(this);
    private int m_keyboardmouseSchemeIndex = -1;
    public InputControlScheme keyboardmouseScheme
    {
        get
        {
            if (m_keyboardmouseSchemeIndex == -1) m_keyboardmouseSchemeIndex = asset.FindControlSchemeIndex("keyboard + mouse");
            return asset.controlSchemes[m_keyboardmouseSchemeIndex];
        }
    }
    public interface IPathfinderTestControlsActions
    {
        void OnMousePos(InputAction.CallbackContext context);
        void OnRightMouseClick(InputAction.CallbackContext context);
        void OnLeftMouseClick(InputAction.CallbackContext context);
    }
}
