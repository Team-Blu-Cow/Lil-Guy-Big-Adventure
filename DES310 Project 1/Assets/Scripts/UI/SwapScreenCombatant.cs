using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SwapScreenCombatant : MonoBehaviour
{
    private void Start()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.0f);
        GetComponentInParent<Canvas>().enabled = false;
        // Enable all screens
        for (int i = 1; i < 6; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Swap(GameObject screen)
    {
        // Disable all screens
        for (int i = 1; i < 6; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        // Re-enable required screen
        screen.SetActive(true);
    }

    public void OpenScreen(PartyCombatant combatant)
    {
        // Enable canvas
        transform.GetComponentInParent<Canvas>().enabled = true;

        // Fade in background
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

        // Set the resistances on ui
        FindObjectOfType<ShowResistanceUI>().SetRes(combatant);

        // Set the name and health on the ui
        GameObject.Find("CombatantName").GetComponent<TextMeshProUGUI>().text = combatant.named + "(" + combatant.GetStats().getStat("HP") + "/" + combatant.GetStats().getStat("Con") + ")";

        // Fade in background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.6f), 0.5f);
        // enable all screens

        // Set stats on text
        SetStats(combatant.GetStats());
        SetStatsModified(combatant.GetStats());
        SetAblities(combatant.GetAbilities());

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
                transform.GetChild(i).gameObject.SetActive(true);
            }
        });

        // Fade out background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0.6f), new Color(0, 0, 0, 0), 0.5f);
    }

    void SetStatsModified(Stats combatantStats)
    {
        GameObject.Find("Stats/ModifiedStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + combatantStats.getStat("Str") + "\n" +
            "Dexterity: " + combatantStats.getStat("Dex") + "\n" +
            "Magic: " + combatantStats.getStat("Mag") + "\n" +
            "Defence: " + combatantStats.getStat("Def") + "\n" +
            "Constitution: " + combatantStats.getStat("Con") + "\n" +
            "Luck: " + combatantStats.getStat("Luck") + "\n" +
            "Speed: " + combatantStats.getStat("Speed") + "\n" +
            "Initit: " + combatantStats.getStat("Init") + "\n";
    }

    void SetStats(Stats combatantStats)
    {
        GameObject.Find("Stats/BaseStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + (combatantStats.getStat("Str") - combatantStats.mod_str) + "\n" +
            "Dexterity: " + (combatantStats.getStat("Dex") - combatantStats.mod_dex) + "\n" +
            "Magic: " + (combatantStats.getStat("Mag") - combatantStats.mod_mag) + "\n" +
            "Defence: " + (combatantStats.getStat("Def") - combatantStats.mod_def) + "\n" +
            "Constitution: " + (combatantStats.getStat("Con") - combatantStats.mod_con) + "\n" +
            "Luck: " + (combatantStats.getStat("Luck") - combatantStats.mod_luck) + "\n" +
            "Speed: " + (combatantStats.getStat("Speed") - combatantStats.mod_speed) + "\n" +
            "Initi: " + (combatantStats.getStat("Init") - combatantStats.mod_init) + "\n";
    }

    void SetAblities(CombatantAbilities abilities)
    {
        int count = 0;
        foreach (Ability ab in abilities.abilitiesUsing)
        {
            GameObject.Find("Base/Abilities").transform.GetChild(count).GetComponentInChildren<TextMeshProUGUI>().text = ab.abilityName;
            GameObject.Find("Base/Abilities").transform.GetChild(count).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Type: " + ab.abilityType.ToString() + "\n" +
            "Main Stat: " + ab.statUsed.ToString() + "\n" +
            "Power: " + ab.abilityPower + "\n" +
            "Area: " + ab.abilityArea + "\n" +
            "Aspect: " + ab.abilityAspect;
            count++;
        }
    }
}
