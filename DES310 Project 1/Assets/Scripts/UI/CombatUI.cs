using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{

    [SerializeField] GameObject BaseGO;
    [SerializeField] private Sprite[] aspectIcons;
    [SerializeField] private Color[] aspectColours;

    GameObject abilityButtons;
    GameObject itemButtons;
    GameObject moveButtons;
    GameObject choiceButtons;

    RectTransform canvas;

    Vector2 combatantPos;

    private void Start()
    {        
        canvas = BaseGO.GetComponent<RectTransform>();

        abilityButtons = BaseGO.transform.GetChild(0).gameObject;
        itemButtons = BaseGO.transform.GetChild(1).gameObject;
        moveButtons = BaseGO.transform.GetChild(2).gameObject;
        choiceButtons = BaseGO.transform.GetChild(3).gameObject;

        aspectIcons = Resources.LoadAll<Sprite>("Sprites/Elements");
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
         choiceButtons.SetActive(false);
    }

    public void deactivateMoveButtons()
    {
        moveButtons.SetActive(false);
    }

    public void deactivateAbilityButtons()
    {
        abilityButtons.SetActive(false);
    }

    public void deactivateItemButtons()
    {
        itemButtons.SetActive(false);
    }

    public void activateChoiceButtons()
    {
        choiceButtons.SetActive(true);
    }

    public void activateChoiceButtons(Vector2 pos)
    {
        List<CombatButton> buttons = new List<CombatButton>();
        buttons.AddRange(choiceButtons.GetComponentsInChildren<CombatButton>());
        

        int offsetY = 50;
        for (int i = 0; i < 3; i++)
        {
            buttons[i].activateButton(new Vector3(110, offsetY, 0), pos);
            offsetY -= 30;
        }

        choiceButtons.SetActive(true);
    }

    public void activateMoveButtons()
    {
    }

    public void activateMoveButtons(Vector2 pos)
    {
        List<CombatButton> buttons = new List<CombatButton>();
        buttons.AddRange(moveButtons.GetComponentsInChildren<CombatButton>());
        
        buttons[0].activateButton(new Vector3(110, -10, 0), pos);
        buttons[1].activateButton(new Vector3(110, 20, 0), pos);
        
        moveButtons.SetActive(true);
    }

    public void activateAbilityButtons()
    {  
        List<CombatButton> buttons = new List<CombatButton>();
        buttons.AddRange(abilityButtons.GetComponentsInChildren<CombatButton>());

        int count = 0;
        foreach (CombatButton button in buttons)
        {
            button.activateButton(button.transform.position, new Vector3());
            if (count < 4)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<BattleManager>().currentCombatant.GetComponent<Combatant>().abilitiesUsing[count].abilityName;
                Aspects.Aspect iconIndex = GetComponent<BattleManager>().currentCombatant.GetComponent<Combatant>().abilitiesUsing[count].abilityAspect;
                button.gameObject.GetComponent<Image>().color = aspectColours[(int)iconIndex];
                button.transform.GetChild(0).GetComponent<Image>().sprite = aspectIcons[GetAbilityIcon(iconIndex)];
                count++;
            }           
        }
        
        abilityButtons.SetActive(true);
    }

    private int GetAbilityIcon(Aspects.Aspect aspect)
    {
        return (int)aspect;
    }

    public void activateItemButtons()
    {       
    }

    public void activateItemButtons(Vector2 pos)
    { 
        List<CombatButton> buttons = new List<CombatButton>();
        buttons.AddRange(itemButtons.GetComponentsInChildren<CombatButton>());
        Item[] items = GetComponent<BattleManager>().currentCombatant.GetComponent<Combatant>().combatantItems;

        int offsetY = 50;
        for (int i = 0; i < items.Length; i++)
        {
            buttons[i].deactivateButton();

            if (items[i])
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = items[i].itemName;
                buttons[i].activateButton(new Vector3(110, offsetY, 0), pos);
                offsetY -= 35;
            }
        }

        buttons[5].activateButton(new Vector3(110, offsetY, 0), pos);
        
        itemButtons.SetActive(true);
    }

    public void useBackButton(Vector2 pos)
    {

        abilityButtons.SetActive(false);
        itemButtons.SetActive(false);       

        activateChoiceButtons(pos);
    }

    public void useBackButton()
    {

        abilityButtons.SetActive(false);
        itemButtons.SetActive(false);
        
    }

    public void colorAbilityButton(int abilityButtonNum)
    {
        //var colors = abilityButtons[abilityButtonNum].GetComponent<Button>().colors;
        //colors.selectedColor = Color.grey;
        //abilityButtons[abilityButtonNum].GetComponent<Button>().colors = colors;
    }
}
