using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [HideInInspector] public InputManager input;
    public Vector3 mousePos = Vector3.zero;
    [HideInInspector] public Transform thisTransform;
    public InitiativeTracker initTracker;
    private GameObject currentCombatant;
    private bool hovering = false;
    private GameObject hoverObject;

    [SerializeField] public BattleManager battleManager;

    void Awake()
    {
        thisTransform = gameObject.GetComponent<Transform>();

        input = new InputManager();
        input.Keyboard.MousePos.performed += ctx => TargetMouse(ctx.ReadValue<Vector2>());
        input.Keyboard.RClick.performed += ctx => MouseRightClick();
        input.Keyboard.LClick.performed += ctx => MouseLeftClick();
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
        mousePos = new Vector3(worldPos.x, worldPos.y, 0);

        battleManager.cursorPos = mousePos;
    }

    void MouseLeftClick()
    {
        //gameObject.GetComponent<DmgPopupTestScript>().CreatePopup(mousePos);
    }

    void MouseRightClick()
    {
        battleManager.RecieveMouseRightClick(mousePos);
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
        /*if (initTracker.getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Attacking)
        {
            hovering = true;
            hoverObject = collision.gameObject;
            initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>().enemy = collision.gameObject;
            Debug.Log(":)");
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if (initTracker.getCurrentCombatant().GetComponent<Combatant>().combatantState== Combatant_State.Attacking)
        {
            hovering = false;
            initTracker.getCurrentCombatant().GetComponent<TestCombatSystem>().enemy = null;
            hoverObject = null;           
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, gameObject.GetComponent<CircleCollider2D>().radius/10);
    }
}
