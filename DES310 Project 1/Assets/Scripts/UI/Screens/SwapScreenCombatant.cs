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
        GetComponentInChildren<ShowResistanceUI>().SetRes(combatant);

        // Set the name and health on the ui
        GameObject.Find("CombatantName").GetComponent<TextMeshProUGUI>().text = combatant.named + "(" + combatant.GetStats().getStat(Combatant_Stats.HP) + "/" + combatant.GetStats().getStat(Combatant_Stats.Constitution) + ")";

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
            "Strength: " + combatantStats.getStat(Combatant_Stats.Strength) + "\n" +
            "Dexterity: " + combatantStats.getStat(Combatant_Stats.Dexterity) + "\n" +
            "Magic: " + combatantStats.getStat(Combatant_Stats.Magic) + "\n" +
            "Defence: " + combatantStats.getStat(Combatant_Stats.Defence) + "\n" +
            "Constitution: " + combatantStats.getStat(Combatant_Stats.Constitution) + "\n" +
            "Luck: " + combatantStats.getStat(Combatant_Stats.Luck) + "\n" +
            "Speed: " + combatantStats.getStat(Combatant_Stats.Speed) + "\n" +
            "Initit: " + combatantStats.getStat(Combatant_Stats.Initiative) + "\n";
    }

    void SetStats(Stats combatantStats)
    {
        GameObject.Find("Stats/BaseStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + (combatantStats.getStat(Combatant_Stats.Strength) - combatantStats.mod_str) + "\n" +
            "Dexterity: " + (combatantStats.getStat(Combatant_Stats.Dexterity) - combatantStats.mod_dex) + "\n" +
            "Magic: " + (combatantStats.getStat(Combatant_Stats.Magic) - combatantStats.mod_mag) + "\n" +
            "Defence: " + (combatantStats.getStat(Combatant_Stats.Defence) - combatantStats.mod_def) + "\n" +
            "Constitution: " + (combatantStats.getStat(Combatant_Stats.Constitution) - combatantStats.mod_con) + "\n" +
            "Luck: " + (combatantStats.getStat(Combatant_Stats.Luck) - combatantStats.mod_luck) + "\n" +
            "Speed: " + (combatantStats.getStat(Combatant_Stats.Speed) - combatantStats.mod_speed) + "\n" +
            "Initi: " + (combatantStats.getStat(Combatant_Stats.Initiative) - combatantStats.mod_init) + "\n";
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
