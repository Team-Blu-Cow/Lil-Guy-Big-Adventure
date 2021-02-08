using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public GameObject[] combatants;
    public InputManager controls;
    public int currentCombatantNum = 1;

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
        
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (combatants[i].GetComponent<Combatant>().combatantNum == currentCombatantNum)
            {
                combatants[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                combatants[i].GetComponent<SpriteRenderer>().enabled = false;
            }
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
