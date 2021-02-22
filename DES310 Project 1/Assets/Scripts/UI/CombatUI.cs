using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{

    [SerializeField] GameObject BaseGO;

    List<CombatButton> abilityButtons = new List<CombatButton>();
    List<CombatButton> itemButtons = new List<CombatButton>();
    List<CombatButton> moveButtons = new List<CombatButton>();
    List<CombatButton> choiceButtons = new List<CombatButton>();

    RectTransform canvas;

    Vector2 combatantPos;

    private void Start()
    {        
        canvas = BaseGO.GetComponent<RectTransform>();

        abilityButtons.AddRange(BaseGO.transform.GetChild(0).GetComponentsInChildren<CombatButton>());
        itemButtons.AddRange(BaseGO.transform.GetChild(1).GetComponentsInChildren<CombatButton>());
        moveButtons.AddRange(BaseGO.transform.GetChild(2).GetComponentsInChildren<CombatButton>());
        choiceButtons.AddRange(BaseGO.transform.GetChild(3).GetComponentsInChildren<CombatButton>());

        for (int i = 0; i < 5; i++)
        {
            abilityButtons[i].abilityButton = true;
        }
    }

    private void Update()
    {

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
        //moveButtons[0].deactivateButton();
    }

    public void deactivateMoveButtons()
    {
        moveButtons[0].deactivateButton();
        moveButtons[1].deactivateButton();
    }

    public void deactivateAbilityButtons()
    {
        for (int j = 0; j < 5; j++)
        {
            abilityButtons[j].deactivateButton();
        }
    }

    public void deactivateItemButtons()
    {
        for (int j = 0; j < 6; j++)
        {
            itemButtons[j].deactivateButton();
        }
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
        //moveButtons[0].activateButton(new Vector3(110, offsetY, 0), pos);
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
        int count = 0;
        foreach (CombatButton button in abilityButtons)
        {
            button.activateButton(button.transform.position, new Vector3());
            if (count < 4)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<BattleManager>().currentCombatant.GetComponent<Combatant>().abilitiesUsing[count].abilityName;
                count++;
            }           
        }
    }

    public void activateItemButtons()
    {
        int offsetY = 110;

        for (int i = 0; i < 6; i++)
        {
            itemButtons[i].activateButton(new Vector3(0, 0, 0), combatantPos);
            offsetY -= 30;
       }
    }

    public void activateItemButtons(Vector2 pos)
    {
        int offsetY = 50;
        for (int i = 0; i < 6; i++)
        {
            itemButtons[i].activateButton(new Vector3(110, offsetY, 0), pos);
            offsetY -= 30;
        }
    }

    public void useBackButton(Vector2 pos)
    {
        for (int i = 0; i < 5; i++)
        {
            abilityButtons[i].deactivateButton();
        }

        for (int i = 0; i < 6; i++)
        {
            itemButtons[i].deactivateButton();
        }

        activateChoiceButtons(pos);
    }

    public void useBackButton()
    {
        for (int i = 0; i < 5; i++)
        {
            abilityButtons[i].deactivateButton();
        }

        for (int i = 0; i < 6; i++)
        {
            itemButtons[i].deactivateButton();
        }
    }

    public void colorAbilityButton(int abilityButtonNum)
    {
        var colors = abilityButtons[abilityButtonNum].GetComponent<Button>().colors;
        colors.selectedColor = Color.grey;
        abilityButtons[abilityButtonNum].GetComponent<Button>().colors = colors;
    }
}
