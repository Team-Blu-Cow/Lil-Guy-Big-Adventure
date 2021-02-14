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
    private bool hovering = false;
    private GameObject hoverObject;

    void Awake()
    {
        thisTransform = gameObject.GetComponent<Transform>();

        input = new InputManager();
        input.Keyboard.MousePos.performed += ctx => TargetMouse(ctx.ReadValue<Vector2>());
        input.Keyboard.RClick.performed += ctx => MouseMove();
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
        if (initTracker.getCurrentCombatant() != null)
        {
            Vector2 worldPos;
            worldPos = Camera.main.ScreenToWorldPoint(position);
            mousePos = new Vector3(worldPos.x, worldPos.y, -1);
        }

    }

    void MouseMove()
    {
        if (initTracker.getCurrentCombatant() != null)
        {
            if (initTracker.getCurrentCombatant().GetComponent<PathFindingUnit>() != null)
            {
                if (initTracker.getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Idle)
                {
                    initTracker.getCurrentCombatant().GetComponent<PathFindingUnit>().RequestPath();
                    initTracker.getCurrentCombatant().GetComponent<Combatant>().combatantState = Combatant_State.Moving;
                    initTracker.getCurrentCombatant().GetComponent<Combatant>().oldPosition = initTracker.getCurrentCombatant().GetComponent<Transform>().position;
                }
            }

            if(initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>() != null)
            {              
                if(hovering == true)
                {
                    Debug.Log("khjdsf");
                }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (initTracker.getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Attacking)
        {
            hovering = true;
            hoverObject = collision.gameObject;
            initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>().enemy = collision.gameObject;
            Debug.Log(":)");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (initTracker.getCurrentCombatant().GetComponent<Combatant>().combatantState== Combatant_State.Attacking)
        {
            hovering = false;
            initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>().enemy = null;
            hoverObject = null;           
        }
    }
}
