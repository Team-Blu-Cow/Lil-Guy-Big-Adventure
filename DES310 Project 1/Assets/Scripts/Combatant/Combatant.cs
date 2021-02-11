using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    public int combatantNum;
    public bool fighting;

    public Ability[] abilitiesLearnt;
    public Ability[] abilitiesUsing;
    public int abilitySelect = 0;

    public Item[] combatantItems;
    public int currentItem = 0;

    public float[] resistances;
    public Quirks[] combatantQuirks;

    public InitiativeTracker initTracker;

    // TODO Split these into enum for each state for the combatant
    public bool moved = false; 
    public bool moving = false;
    public Vector3 oldPosition;

    public bool attacking = false;
    public bool attacked = false;

    public InputManager controls;

    /*
     would it not be better to put the Stats class here eg.
     
     public Stats stats = new Stats();
     
     this would save us having to use GetComponent<Stats>() every time you need access
     */

    private void Awake()
    {
        controls = new InputManager();
        controls.Keyboard.IncrementAbilitySelect.started += ctx => changeItem(1);
        controls.Keyboard.DecrementAbilitySelect.started += ctx => changeItem(-1);
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

        initTracker.AddCombatant(this.gameObject);
    }

    private void Update()
    {
        
        if(moving == true)
        {
            Transform targetT = GetComponent<PathFindingUnit>().target;

            // TODO rethinkg this if statement. Looks ugly :( (Sorry if statement)
            if(transform.position.x > targetT.position.x - 0.5f && transform.position.x < targetT.position.x + 0.5f && transform.position.y > targetT.position.y - 0.5f && transform.position.y < targetT.position.x + 0.5f)
            {
                moved = true;
                moving = false;               
            }
        }
    }

    public void SetAbilityUsed(int abilityNum)
    {
        abilitiesUsing[abilitySelect] = abilitiesLearnt[abilityNum];
    }

    private void changeItem(int itemSwitch)
    {
        if ((itemSwitch == 1 && currentItem < 4) || (itemSwitch == -1 && currentItem > 0))
        {
            currentItem += itemSwitch;
        }
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
        if (moved == true)
        {
            moved = false;
            moving = false;
            transform.position = oldPosition;
        }
    }

    public void confirmMove()
    {
        moved = true;
        attacking = true;
    }

    public void attackAbility(int abilityNum)
    {
        GetComponent<TestCombatSystem>().CastAbility(abilityNum);
        attacked = true;
        attacking = false;
        initTracker.ChangeCurrentCombatant();
    }
}

