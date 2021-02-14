using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    public CombatButton[] moveButtons;
    public CombatButton[] abilityButtons;
    public CombatButton[] itemButtons;
    public CombatButton[] choiceButtons;

    InitiativeTracker initTracker;
    Vector3 combatantPos;
    Vector3 combatantWorldPos;

    private void Start()
    {
        initTracker = GetComponent<InitiativeTracker>();
        for(int i = 0; i < 4; i++)
        {
            abilityButtons[i].abilityButton = true;
        }
        choiceButtons[3].abilityButton = true;
    }

    private void Update()
    {
        combatantWorldPos = Camera.main.WorldToScreenPoint(initTracker.getCurrentCombatant().transform.position);
        combatantPos = new Vector3(combatantWorldPos.x, combatantWorldPos.y, -1);
    }

    public void deactivateChoiceButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            choiceButtons[i].deactivateButton();
        }
    }

    public void deactivateMoveButtons()
    {
        moveButtons[0].deactivateButton();
        moveButtons[1].deactivateButton();
    }

    public void deactivateAbilityButtons()
    {
        for (int j = 0; j < 4; j++)
        {
            abilityButtons[j].deactivateButton();
        }
        choiceButtons[3].deactivateButton();
    }

    public void deactivateItemButtons()
    {
        for (int j = 0; j < 5; j++)
        {
            itemButtons[j].deactivateButton();
        }
        choiceButtons[4].deactivateButton();
    }

    public void activateChoiceButtons()
    {
        int offsetY = 50;
        for (int i = 0; i < 3; i++)
        {
            choiceButtons[i].activateButton(new Vector3(110, offsetY, 0), combatantPos);
            offsetY -= 30;
        }
    }

    public void activateMoveButtons()
    {
        moveButtons[0].activateButton(new Vector3(110, -10, 0), combatantPos);
        moveButtons[1].activateButton(new Vector3(110, 20, 0), combatantPos);
    }

    public void activateAbilityButtons()
    {
        int offsetY = -30;

        for (int i = 0; i < 4; i++)
        {
            abilityButtons[i].activateButton(new Vector3(-110, offsetY, 0), combatantPos);
            offsetY -= 30;
        }

        choiceButtons[3].activateButton(new Vector3(-110, offsetY, 0), combatantPos);
    }

    public void activateItemButtons()
    {
        int offsetY = 110;

        for (int i = 0; i < 5; i++)
        {
            itemButtons[i].activateButton(new Vector3(110, offsetY, 0), combatantPos);
            offsetY -= 30;
        }

        choiceButtons[4].activateButton(new Vector3(110, offsetY, 0), combatantPos);
    }

    public void useBackButton()
    {
        for (int i = 0; i < 4; i++)
        {
            abilityButtons[i].deactivateButton();
        }

        for (int i = 0; i < 5; i++)
        {
            itemButtons[i].deactivateButton();
        }

        choiceButtons[3].deactivateButton();
        choiceButtons[4].deactivateButton();
    }
}
