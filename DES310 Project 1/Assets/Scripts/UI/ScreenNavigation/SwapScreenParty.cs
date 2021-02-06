using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SwapScreenParty : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set starting to closed position
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.0f);
        GetComponentInParent<Canvas>().enabled = false;
    }

    // Open the screen and set it up
    public void OpenScreen()
    {
        // Enable canvas
        transform.GetComponentInParent<Canvas>().enabled = true;

        // Scale in the screen
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

        // Fade in the black background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.6f), 0.5f);

        // Set what party members appear
        for(int i = 0; i <  GameObject.Find("InGameCanvas/Party").transform.childCount; i++)
        {
            SetPartyMember(i);
            transform.GetChild(0).GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = transform.GetChild(0).GetComponentsInChildren<PartyCombatant>()[i].named;
        }

    }

    // Close the whole screen
    public void CloseScreen()
    {
        // Scale out the screen and disable once finished and hide when done
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(() => GetComponentInParent<Canvas>().enabled = false);

        // Fade out the black background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0.6f), new Color(0, 0, 0, 0), 0.5f);
    }

    void SetPartyMember(int i)
    {
        // get the child in he party section in the "InGameCanvas"
        PartyCombatant combatant = GameObject.Find("InGameCanvas/Party").GetComponentsInChildren<PartyCombatant>()[i];
        transform.GetChild(0).GetComponentsInChildren<PartyCombatant>()[i].SetAll(combatant.GetCombatantGO(),combatant.GetStats(),combatant.GetCombatant(), combatant.named);
    }
}
