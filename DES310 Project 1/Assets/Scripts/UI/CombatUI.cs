using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    public CombatButton[] moveButtons;
    public CombatButton[] abilityButtons;
    public CombatButton[] itemButtons;
    public CombatButton[] choiceButtons;
    public RectTransform canvas;

    InitiativeTracker initTracker;
    Vector2 combatantPos;
    Vector2 combatantScreenPos;

    private void Start()
    {
        //initTracker = gameObject.GetComponent<InitiativeTracker>();
        for (int i = 0; i < 4; i++)
        {
            abilityButtons[i].abilityButton = true;
        }
        choiceButtons[3].abilityButton = true;
    }

    private void Update()
    {
        //combatantScreenPos = Camera.main.WorldToViewportPoint(initTracker.getCurrentCombatant().transform.position);
        //combatantPos = new Vector2((combatantScreenPos.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f), (combatantScreenPos.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        // transform.GetChild(0);
    }

    public Vector2 WorldToCanvasSpace(Vector3 worldPos)
    {
        Vector2 viewPortPos = Camera.main.WorldToViewportPoint(worldPos);
        Vector2 canvasPos = new Vector2((viewPortPos.x * canvas.sizeDelta.x) - (canvas.sizeDelta.x * 0.5f), (viewPortPos.y * canvas.sizeDelta.y) - (canvas.sizeDelta.y * 0.5f));
        return canvasPos;
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

    public void activateChoiceButtons(Vector2 pos)
    {
        int offsetY = 50;
        for (int i = 0; i < 3; i++)
        {
            choiceButtons[i].activateButton(new Vector3(110, offsetY, 0), pos);
            offsetY -= 30;
        }
    }

    public void activateMoveButtons()
    {
        moveButtons[0].activateButton(new Vector3(110, -10, 0), combatantPos);
        moveButtons[1].activateButton(new Vector3(110, 20, 0), combatantPos);
    }

    public void activateMoveButtons(Vector2 pos)
    {
        moveButtons[0].activateButton(new Vector3(110, -10, 0), pos);
        moveButtons[1].activateButton(new Vector3(110, 20, 0), pos);
    }

    public void activateAbilityButtons()
    {
        abilityButtons[0].activateButton(new Vector3(-165, 100, 0), new Vector3(0, 0, 0));
        abilityButtons[1].activateButton(new Vector3(165, 100, 0), new Vector3(0, 0, 0));
        abilityButtons[2].activateButton(new Vector3(-165, 35, 0), new Vector3(0, 0, 0));
        abilityButtons[3].activateButton(new Vector3(165, 35, 0), new Vector3(0, 0, 0));
        choiceButtons[3].activateButton(new Vector3(0, 155, 0), new Vector3(0, 0, 0));
    }

    public void activateItemButtons()
    {
        int offsetY = 110;

        for (int i = 0; i < 5; i++)
        {
            itemButtons[i].activateButton(new Vector3(0, 0, 0), combatantPos);
            offsetY -= 30;
        }

        choiceButtons[4].activateButton(new Vector3(110, offsetY, 0), combatantPos);
    }

    public void activateItemButtons(Vector2 pos)
    {
        int offsetY = 50;
        for (int i = 0; i < 5; i++)
        {
            itemButtons[i].activateButton(new Vector3(110, offsetY, 0), pos);
            offsetY -= 30;
        }

        choiceButtons[4].activateButton(new Vector3(110, offsetY, 0), pos);
    }

    public void useBackButton(Vector2 pos)
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

        activateChoiceButtons(pos);
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

    public void colorAbilityButton(int abilityButtonNum)
    {
        var colors = abilityButtons[abilityButtonNum].GetComponent<Button>().colors;
        colors.selectedColor = Color.grey;
        abilityButtons[abilityButtonNum].GetComponent<Button>().colors = colors;
    }
}
