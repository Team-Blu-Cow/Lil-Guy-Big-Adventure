using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SwapScreenCombatant : MonoBehaviour
{
    public GameObject swapAbility;

    private void Start()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.0f);
        GetComponentInParent<Canvas>().enabled = false;
        // Enable all screens
        for (int i = 1; i < 6; i++)
        {
            transform.GetChild(i).GetComponent<Canvas>().enabled = true;
        }
    }

    public void Swap(GameObject screen)
    {
        // Disable all screens
        for (int i = 1; i < 6; i++)
        {
            transform.GetChild(i).GetComponent<Canvas>().enabled = false;
        }
        // Re-enable required screen
        screen.GetComponent<Canvas>().enabled = true;
    }

    public void OpenScreen(PartyCombatant combatant)
    {
        // Enable canvas
        transform.GetComponentInParent<Canvas>().enabled = true;
        GetComponent<PartyCombatant>().SetCombatant(combatant.GetCombatant());
        GetComponent<PartyCombatant>().SetCombatantGO(combatant.combatantGO);

        // Fade in background
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

        // Set the resistances on ui
        GetComponentInChildren<ShowResistanceUI>().SetRes(combatant.GetCombatant());

        // Set the name and health on the ui
        GameObject.Find("CombatantName").GetComponent<TextMeshProUGUI>().text = combatant.named + "(" + combatant.GetStats().GetStat(Combatant_Stats.HP) + "/" + combatant.GetStats().GetStat(Combatant_Stats.Constitution) + ")";

        // Fade in background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.6f), 0.5f);

        // Set stats on text
        SetStats(combatant.GetStats());
        SetStatsModified(combatant.GetStats());
        SetAblities(combatant.GetCombatant());
        int count = 0;
        foreach (LearnedAbility ability in swapAbility.GetComponentsInChildren<LearnedAbility>())
        {
            ability.SetAbility(combatant.GetCombatant(),count);
            count++;
        }

        // Swap to main screen
        Swap(transform.GetChild(1).gameObject);
    }

    public void CloseScreen()
    {
        // Scale out and hide when finsished
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(() =>
        {
            GetComponentInParent<Canvas>().enabled = false;
            // Enabe all screens
            for (int i = 1; i < 6; i++)
            {
                transform.GetChild(i).GetComponent<Canvas>().enabled = true;
            }
        });

        // Fade out background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0.6f), new Color(0, 0, 0, 0), 0.5f);
    }

    public void ToggleLearned(bool toggle)
    {
        swapAbility.GetComponent<Canvas>().enabled = toggle;

        foreach (Canvas canvas in swapAbility.GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = toggle;
        }
    }
    
    public void FlipLearned()
    {
        foreach (Canvas canvas in swapAbility.GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = !canvas.enabled;
        }
    }

    void SetStatsModified(Stats combatantStats)
    {
        GameObject.Find("Stats/ModifiedStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + combatantStats.GetFinalStat(Combatant_Stats.Strength) + "\n" +
            "Dexterity: " + combatantStats.GetFinalStat(Combatant_Stats.Dexterity) + "\n" +
            "Magic: " + combatantStats.GetFinalStat(Combatant_Stats.Magic) + "\n" +
            "Defence: " + combatantStats.GetFinalStat(Combatant_Stats.Defence) + "\n" +
            "Constitution: " + combatantStats.GetFinalStat(Combatant_Stats.Constitution) + "\n" +
            "Luck: " + combatantStats.GetFinalStat(Combatant_Stats.Luck) + "\n" +
            "Speed: " + combatantStats.GetFinalStat(Combatant_Stats.Speed) + "\n" +
            "Initit: " + combatantStats.GetFinalStat(Combatant_Stats.Initiative) + "\n";
    }

    void SetStats(Stats combatantStats)
    {
        GameObject.Find("Stats/BaseStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + (combatantStats.GetStat(Combatant_Stats.Strength) ) + "\n" +
            "Dexterity: " + (combatantStats.GetStat(Combatant_Stats.Dexterity) ) + "\n" +
            "Magic: " + (combatantStats.GetStat(Combatant_Stats.Magic)) + "\n" +
            "Defence: " + (combatantStats.GetStat(Combatant_Stats.Defence) ) + "\n" +
            "Constitution: " + (combatantStats.GetStat(Combatant_Stats.Constitution) ) + "\n" +
            "Luck: " + (combatantStats.GetStat(Combatant_Stats.Luck) ) + "\n" +
            "Speed: " + (combatantStats.GetStat(Combatant_Stats.Speed) ) + "\n" +
            "Initi: " + (combatantStats.GetStat(Combatant_Stats.Initiative) ) + "\n";
    }

    public void SetAblities(Combatant abilities)
    {
        int count = 0;
        
        GameObject abilityGO = GameObject.Find("Base/Abilities");
        for (int i = 0; i < 4; i++)
        {
            abilityGO.transform.GetChild(i).transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "";
            abilityGO.transform.GetChild(i).transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
        }


        foreach (Ability ab in abilities.abilitiesUsing)
        {
            if (ab)
            {
                abilityGO.transform.GetChild(count).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = ab.abilityName;
                abilityGO.transform.GetChild(count).GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =
                "Type: " + ab.abilityType.ToString() + "\n" +
                "Main Stat: " + ab.statUsed.ToString() + "\n" +
                "Power: " + ab.abilityPower + "\n" +
                "Area: " + ab.abilityArea + "\n" +
                "Aspect: " + ab.abilityAspect;
                count++;
            }
        }
    }
}
