using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    public Ability[] abilitiesLearnt;
    public Ability[] abilitiesUsing;
    public int abilitySelect = 0;

    public Item[] combatantItems;
    public int currentItem = 0;

    public Aspects.Aspect[] resistances;
    public Aspects.Aspect[] immunities;
    public Aspects.Aspect[] vulnerabilities;

    public Quirks[] combatantQuirks;

    public InputManager controls;

    private void Awake()
    {
        controls = new InputManager();
        controls.Keyboard.ChangeItem.started += ctx => changeItem(ctx.ReadValue<int>());
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
        currentItem += itemSwitch;
    }

    private void changeAbilitySelect(int num)
    {
        abilitySelect = num;
    }
}
