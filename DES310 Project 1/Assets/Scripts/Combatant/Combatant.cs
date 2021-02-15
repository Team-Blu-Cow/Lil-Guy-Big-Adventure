using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Combatant_State
{
    Idle,
    Moving,
    Moved,
    Attacking,
    Attacked,
}

public class Combatant : MonoBehaviour
{
    public int combatantNum;
    public bool fighting;

    public Ability[] abilitiesLearnt;
    public Ability[] abilitiesUsing;
    public int abilitySelect = 0;

    public Item[] combatantItems;

    public float[] resistances;
    public Quirks[] combatantQuirks;

    public InitiativeTracker initTracker;

    public Vector3 oldPosition;
    public Combatant_State combatantState = Combatant_State.Idle;

    public InputManager controls;

    private Animator animator;

    /*
     would it not be better to put the Stats class here eg.
     
     public Stats stats = new Stats();
     
     this would save us having to use GetComponent<Stats>() every time you need access
     */

    private void Awake()
    {
        controls = new InputManager();
        controls.Keyboard.Changetoability1.started += ctx => changeAbilitySelect(0);
        controls.Keyboard.Changetoability2.started += ctx => changeAbilitySelect(1);
        controls.Keyboard.Changetoability3.started += ctx => changeAbilitySelect(2);
        controls.Keyboard.Changetoability4.started += ctx => changeAbilitySelect(3);
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        abilitiesUsing[0] = abilitiesLearnt[0];
        abilitiesUsing[1] = abilitiesLearnt[1];
        abilitiesUsing[2] = abilitiesLearnt[2];
        abilitiesUsing[3] = abilitiesLearnt[3];

        //initTracker.AddCombatant(this.gameObject);
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        
        if(combatantState == Combatant_State.Moving)
        {
            Transform targetT = GetComponent<PathFindingUnit>().target;

            // TODO rethinkg this if statement. Looks ugly :( (Sorry if statement)
            if(transform.position.x > targetT.position.x - 0.5f && transform.position.x < targetT.position.x + 0.5f && transform.position.y > targetT.position.y - 0.5f && transform.position.y < targetT.position.x + 0.5f)
            {
                combatantState = Combatant_State.Moved;   
            }
        }
    }

    public void SetAbilityUsed(int abilityNum)
    {
        abilitiesUsing[abilitySelect] = abilitiesLearnt[abilityNum];
    }

    private void changeAbilitySelect(int num)
    {
        abilitySelect = num;
    }

    public void do_damage(int damage, Aspects.Aspect type)
    {
        // why cant i just get direct access to this? the switch case in these functions adds unnecessary overhead
        GetComponent<Stats>().setStat(Combatant_Stats.HP, GetComponent<Stats>().getStat(Combatant_Stats.HP) - damage);
    }

    public void do_heal(int heal)
    {
        GetComponent<Stats>().setStat(Combatant_Stats.HP, GetComponent<Stats>().getStat(Combatant_Stats.HP) + heal);
    }

    public void cancelMove()
    {
        if(combatantState == Combatant_State.Moved)
        {
            combatantState = Combatant_State.Idle;
            transform.position = oldPosition;
        }
    }

    public void confirmMove()
    {
        combatantState = Combatant_State.Attacking;
    }

    public void attackAbility(int abilityNum)
    {
        GetComponent<TestCombatSystem>().CastAbility(abilityNum);
        combatantState = Combatant_State.Attacked;
        initTracker.ChangeCurrentCombatant();       
    }

    public void UseItem(int itemNum)
    {
        GetComponent<TestCombatSystem>().UseItem(itemNum);
        combatantState = Combatant_State.Attacked;
        initTracker.ChangeCurrentCombatant();
    }
}

