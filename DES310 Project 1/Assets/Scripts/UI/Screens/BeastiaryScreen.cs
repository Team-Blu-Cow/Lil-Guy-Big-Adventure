using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeastiaryScreen : MonoBehaviour
{
    public List<GameObject> EnemiesMet;
    public GameObject EnemyPrefab;

    [Header ("Pages")]
    public GameObject overview;
    public GameObject combatant;

    [Header ("Flip Page Objects")]
    public GameObject leftPage;
    public GameObject rightPage;
    public GameObject newLeftPage;
    public GameObject newRightPage;

    int currentCombatant = 0;

    enum PageOrder
    {
        newLeft,
        right,
        left,
        newRight
    }

    public void FlipPageForward()
    {
        if (currentCombatant + 1 < EnemiesMet.Count)
        {
            leftPage.transform.localScale = new Vector3(0, 1, 1);
            newRightPage.transform.localScale = new Vector3(1, 1, 1);

            LeanTween.cancelAll();
            LeanTween.scaleX(newRightPage, 0, 0.5f);
            LeanTween.delayedCall(0.5f, RightStart);
       
            currentCombatant++;   
            SetLeft(true);  
            SetRight(true);
        }
    }

    public void FlipPageBackward()
    {
        if (currentCombatant-1 >= 0)
        {
            newRightPage.transform.localScale = new Vector3(0, 1, 1);
            leftPage.transform.localScale = new Vector3(1, 1, 1);

            LeanTween.cancelAll();
            LeanTween.scaleX(leftPage, 0, 0.5f);
            LeanTween.delayedCall(0.5f, LeftStart);

            currentCombatant--;
            SetLeft(false);
            SetRight(false);            
        }
    }

    void RightStart()
    {
        LeanTween.scaleX(leftPage, 1, 0.5f).setOnComplete(SetTexts);
    }

    void LeftStart()
    {
        LeanTween.scaleX(newRightPage, 1, 0.5f).setOnComplete(SetTexts);
    }

    private void SetTexts()
    {
        SetLeft(false);
        SetRight(true);
        SetLeft(true);
        SetRight(false);
    }

    void SetLeft(bool newPage)
    {
        int setNew;
        if (newPage)
            setNew = (int)PageOrder.left;
        else
            setNew = (int)PageOrder.newLeft;

        Stats combatantStats = EnemiesMet[currentCombatant].GetComponent<Stats>();

        foreach (TextMeshProUGUI text in combatant.transform.GetChild(setNew).GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name == "StatText")
            {
                text.text =
                "Strength: " + (combatantStats.getStat(Combatant_Stats.Strength) - combatantStats.mod_str) + "\n" +
                "Dexterity: " + (combatantStats.getStat(Combatant_Stats.Dexterity) - combatantStats.mod_dex) + "\n" +
                "Magic: " + (combatantStats.getStat(Combatant_Stats.Magic) - combatantStats.mod_mag) + "\n" +
                "Defence: " + (combatantStats.getStat(Combatant_Stats.Defence) - combatantStats.mod_def) + "\n" +
                "Constitution: " + (combatantStats.getStat(Combatant_Stats.Constitution) - combatantStats.mod_con) + "\n" +
                "Luck: " + (combatantStats.getStat(Combatant_Stats.Luck) - combatantStats.mod_luck) + "\n" +
                "Speed: " + (combatantStats.getStat(Combatant_Stats.Speed) - combatantStats.mod_speed) + "\n" +
                "Init: " + (combatantStats.getStat(Combatant_Stats.Initiative) - combatantStats.mod_init) + "\n";
            }
            else if (text.name == ("CreatureName"))
            {
                text.text = combatantStats.combatant_type.ToString();
            }
        }

        combatant.transform.GetChild(setNew).GetComponentsInChildren<Image>()[1].sprite = EnemiesMet[currentCombatant].GetComponent<SpriteRenderer>().sprite;
        combatant.transform.GetChild(setNew).GetComponentsInChildren<Image>()[1].SetNativeSize();
    }
    
    void SetRight(bool newPage)
    {
        int setNew;
        if (newPage)
            setNew = (int)PageOrder.right;
        else
            setNew = (int)PageOrder.newRight;

        foreach (TextMeshProUGUI text in combatant.transform.GetChild(setNew).GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name == ("Lore"))
            {
                text.text = "This is the lore description";
            }
        }

        Combatant test = EnemiesMet[currentCombatant].GetComponent<Combatant>();
        combatant.transform.GetChild(setNew).GetComponentInChildren<ShowResistanceUI>().SetRes(test);
    }
    
    public void OpenScreen()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void CloseScreen()
    {
        GetComponent<Canvas>().enabled = false;
    }
    
    public bool ToggleScreen()
    {
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = !canvas.enabled;
        }
        UpdateSeen();
        return GetComponent<Canvas>().enabled;
    }

    public void OpenCombatant(int i)
    {
        currentCombatant = i;
        SetTexts();

        overview.GetComponent<Canvas>().enabled = false;
        combatant.GetComponent<Canvas>().enabled = true;
    }

    public void CloseCreature()
    {
        combatant.GetComponent<Canvas>().enabled = false;
        overview.GetComponent<Canvas>().enabled = true;
    }

    public void UpdateSeen()
    {
        foreach (Transform child in overview.transform.GetChild(2).GetComponentsInChildren<Transform>())
        {
            if(child.name != "Creatures")
                Destroy(child.gameObject);
        }

        for (int i = 0; i < EnemiesMet.Count; i++)
        {
            GameObject newCombatant = Instantiate(EnemyPrefab, overview.transform.GetChild(2));
            int x = new int();
            x = i;
            newCombatant.GetComponent<Button>().onClick.AddListener(() => { OpenCombatant(x); });

            newCombatant.GetComponentInChildren<TextMeshProUGUI>().text = EnemiesMet[x].GetComponent<Stats>().combatant_type.ToString();
            newCombatant.GetComponentInChildren<Image>().sprite = EnemiesMet[x].GetComponent<SpriteRenderer>().sprite;
        }        
    }
}
