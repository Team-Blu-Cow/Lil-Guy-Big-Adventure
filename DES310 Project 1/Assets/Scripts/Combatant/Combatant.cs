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
    }

    private void Update()
    {

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


    // TODO - this is here to as a basic placeholder, it should be moved into its own file
    public enum DamageType
    {
        NORMAL = 0,
        FIRE,
        WATER
    };

    public void do_damage(int damage, DamageType type)
    {
        // why cant i just get direct access to this? the switch case in these functions adds unnecessary overhead
        GetComponent<Stats>().setStat("HP", -damage);
    }

    public void do_heal(int heal)
    {
        GetComponent<Stats>().setStat("HP", heal);
    }
}

