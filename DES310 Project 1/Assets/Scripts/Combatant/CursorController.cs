using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [HideInInspector] public InputManager input;
    [HideInInspector] public Vector3 mousePos;
    [HideInInspector] public Transform thisTransform;
    public InitiativeTracker initTracker;
    private GameObject currentCombatant;

    void Awake()
    {
        thisTransform = gameObject.GetComponent<Transform>();

        input = new InputManager();
        input.Keyboard.MousePos.performed += ctx => TargetMouse(ctx.ReadValue<Vector2>());
        input.Keyboard.MouseClick.performed += ctx => MouseMove();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void TargetMouse(Vector2 position)
    {
        Vector2 worldPos;
        worldPos = Camera.main.ScreenToWorldPoint(position);
        mousePos = new Vector3(worldPos.x, worldPos.y, -1);
    }

    void MouseMove()
    {
        Debug.Log("Clickety Clack");
        if (initTracker.getCurrentCombatant() != null)
        {
            Debug.Log("Found Combatant");
            if (initTracker.getCurrentCombatant().GetComponent<PathFindingUnit>() != null)
            {
                Debug.Log("Finding Path");
                initTracker.getCurrentCombatant().GetComponent<PathFindingUnit>().RequestPath();
            }
        }
    }

    private void Update()
    {
        if (mousePos != null)
        {
            thisTransform.position = mousePos;
        }
    }


}
