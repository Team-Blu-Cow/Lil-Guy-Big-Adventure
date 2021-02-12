using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject[] combatants;
    public InputManager controls;
    public int currentCombatantNum = 1;

    Color color;
    Color white;

    private void Awake()
    {
        controls = new InputManager();
        controls.Keyboard.ChangePartyMember.started += ctx => ChangeSelectedPartyMember();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        white.r = 255;
        white.g = 255;
        white.b = 255;
        white.a = 255;
        color.r = 255;
        color.g = 0;
        color.b = 0;
        color.a = 255;
    }

    void Update()
    {

        for(int i = 0; i < 4; i++)
        {
            //if(currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum)
            //{
            //    combatants[i].GetComponent<SpriteRenderer>().color = color;
            //    combatants[i].GetComponent<Combatant>().fighting = true;
            //}
            //else
            //{
            //    combatants[i].GetComponent<SpriteRenderer>().color = white;
            //    combatants[i].GetComponent<Combatant>().fighting = false;
            //}
        }
    }

    void ChangeSelectedPartyMember()
    {
        currentCombatantNum++;
        if(currentCombatantNum > 4)
        {
            currentCombatantNum = 1;
        }
    }

    public void combatantFight(int abilityNum)
    {
        combatants[currentCombatantNum - 1].GetComponent<TestCombatSystem>().CastAbility(abilityNum);
        Debug.Log("Combatant " + currentCombatantNum + " attacked with an ability");
    }

    public void combatantUseItem()
    {
        combatants[currentCombatantNum - 1].GetComponent<TestCombatSystem>().UseItem();
    }

    public void combatantSetAbility(int abilityNum)
    {
        combatants[currentCombatantNum - 1].GetComponent<Combatant>().SetAbilityUsed(abilityNum);
    }
}
