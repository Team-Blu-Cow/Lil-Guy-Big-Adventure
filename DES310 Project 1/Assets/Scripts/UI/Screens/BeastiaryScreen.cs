using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeastiaryScreen : MonoBehaviour
{
    public List<GameObject> EnemiesMet;

    [Header ("Pages")]
    public GameObject overview;
    public GameObject creature;

    [Header ("Flip Page Objects")]
    public GameObject leftPage;
    public GameObject rightPage;
    public GameObject newLeftPage;
    public GameObject newRightPage;

    int currentCreature = 0;

    enum PageOrder
    {
        newLeft,
        right,
        left,
        newRight
    }

    public void FlipPageForward()
    {
        if (currentCreature+1 < EnemiesMet.Count)
        {
            leftPage.transform.localScale = new Vector3(0, 1, 1);
            newRightPage.transform.localScale = new Vector3(1, 1, 1);

            LeanTween.cancelAll();
            LeanTween.scaleX(newRightPage, 0, 0.5f);
            LeanTween.delayedCall(0.5f, RightStart);
       
            currentCreature++;   
            SetLeft(true);  
            SetRight(true);
        }
    }

    public void FlipPageBackward()
    {
        if (currentCreature-1 >= 0)
        {
            newRightPage.transform.localScale = new Vector3(0, 1, 1);
            leftPage.transform.localScale = new Vector3(1, 1, 1);

            LeanTween.cancelAll();
            LeanTween.scaleX(leftPage, 0, 0.5f);
            LeanTween.delayedCall(0.5f, LeftStart);

            currentCreature--;
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

        Stats combatantStats = EnemiesMet[currentCreature].GetComponent<Stats>();

        foreach (TextMeshProUGUI text in creature.transform.GetChild(setNew).GetComponentsInChildren<TextMeshProUGUI>())
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
    }
    
    void SetRight(bool newPage)
    {
        int setNew;
        if (newPage)
            setNew = (int)PageOrder.right;
        else
            setNew = (int)PageOrder.newRight;

        foreach (TextMeshProUGUI text in creature.transform.GetChild(setNew).GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (text.name == ("Lore"))
            {
                text.text = "This is the lore description";
            }
        }

        Combatant test = EnemiesMet[currentCreature].GetComponent<Combatant>();
        creature.transform.GetChild(setNew).GetComponentInChildren<ShowResistanceUI>().SetRes(test);
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
        GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
        return GetComponent<Canvas>().enabled;
    }

    public void OpenCreature(int i)
    {
        currentCreature = i;
        SetTexts();

        overview.GetComponent<Canvas>().enabled = false;
        creature.GetComponent<Canvas>().enabled = true;
    }

    public void CloseCreature()
    {
        creature.GetComponent<Canvas>().enabled = false;
        overview.GetComponent<Canvas>().enabled = true;
    }
}
