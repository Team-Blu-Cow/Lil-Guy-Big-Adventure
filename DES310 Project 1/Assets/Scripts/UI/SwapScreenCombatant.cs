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

    }

    public void CloseScreen()
    {
        // Scale out and hide when finsished
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(() =>
        {
            GetComponentInParent<Canvas>().enabled = false;
            Swap(transform.GetChild(1).gameObject);
        });

        // Fade out background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0.6f), new Color(0, 0, 0, 0), 0.5f);
    }
}
