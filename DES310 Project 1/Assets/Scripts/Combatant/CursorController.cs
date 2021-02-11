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
        if (initTracker.getCurrentCombatant() != null)
        {
            Vector2 worldPos;
            worldPos = Camera.main.ScreenToWorldPoint(position);
            mousePos = new Vector3(worldPos.x, worldPos.y, -1);
        }

    }

    void MouseMove()
    {
        Debug.Log("Clickety Clack");
        if (initTracker.getCurrentCombatant() != null)
        {
            Debug.Log("Found Combatant");
            if (initTracker.getCurrentCombatant().GetComponent<PathFindingUnit>() != null)
            {
                if (initTracker.getCurrentCombatant().GetComponent<Combatant>().moved != true)
                {
                    Debug.Log("Finding Path");
                    initTracker.getCurrentCombatant().GetComponent<PathFindingUnit>().RequestPath();
                    initTracker.getCurrentCombatant().GetComponent<Combatant>().moving = true;
                    initTracker.getCurrentCombatant().GetComponent<Combatant>().oldPosition = initTracker.getCurrentCombatant().GetComponent<Transform>().position;
                }
            }

            if(initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>() != null)
            {
                if(hovering == true)
                {
                    initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>().enemy = hoverObject;
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
        if (initTracker.getCurrentCombatant().GetComponent<Combatant>().attacking == true)
        {
            hovering = true;
            hoverObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (initTracker.getCurrentCombatant().GetComponent<Combatant>().attacking == true)
        {
            hovering = false;
            hoverObject = null;
        }
    }
}
