using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SwapScreenCombatant : MonoBehaviour
{
    PartyCombatant combatant;

    private void Start()
    {
        combatant = GetComponent<PartyCombatant>();
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.0f);
        GetComponentInParent<Canvas>().enabled = false;
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

    public void OpenScreen(PartyCombatant combatantIn)
    {
        // Enable canvas
        transform.GetComponentInParent<Canvas>().enabled = true;

        // Fade in background
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

        // Setting combatant info:TO BE REPLACED
        combatant = combatantIn;

        // Set the resistances on ui
        //FindObjectOfType<ShowResistanceUI>().SetRes(combatant);

        // Set the name and health on the ui
        GameObject.Find("CombatantName").GetComponent<TextMeshProUGUI>().text = combatant.named + "(" + combatant.combatantStats.getStat("HP") + "/" + combatant.combatantStats.getStat("Con") + ")";

        // Fade in background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.6f), 0.5f);
        // enable all screens
       
        // Set stats on text
        SetStats();
        SetStatsModified();

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

    void SetStats()
    {
        GameObject.Find("Stats/BaseStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + combatant.combatantStats.getStat("Str") + "\n" +
            "Dexterity: " + combatant.combatantStats.getStat("Dex") + "\n" +
            "Magic: " + combatant.combatantStats.getStat("Mag") + "\n" +
            "Defence: " + combatant.combatantStats.getStat("Def") + "\n" +
            "Constitution: " + combatant.combatantStats.getStat("Con") + "\n" +
            "Luck: " + combatant.combatantStats.getStat("Luck") + "\n" +
            "Speed: " + combatant.combatantStats.getStat("Speed") + "\n" +
            "Initit: " + combatant.combatantStats.getStat("Init") + "\n";
    }
    
    void SetStatsModified()
    {
        GameObject.Find("Stats/ModifiedStats").transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
            "Strength: " + combatant.combatantStats.getStat("Str") * combatant.combatantStats.mod_str + "\n" +
            "Dexterity: " + combatant.combatantStats.getStat("Dex") * combatant.combatantStats.mod_str + "\n" +
            "Magic: " + combatant.combatantStats.getStat("Mag") * combatant.combatantStats.mod_mag + "\n" +
            "Defence: " + combatant.combatantStats.getStat("Def") * combatant.combatantStats.mod_def + "\n" +
            "Constitution: " + combatant.combatantStats.getStat("Con") * combatant.combatantStats.mod_con + "\n" +
            "Luck: " + combatant.combatantStats.getStat("Luck") * combatant.combatantStats.mod_luck + "\n" +
            "Speed: " + combatant.combatantStats.getStat("Speed") * combatant.combatantStats.mod_speed + "\n" +
            "Initi: " + combatant.combatantStats.getStat("Init") * combatant.combatantStats.mod_init + "\n";
    }
}
