using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwapScreen : MonoBehaviour
{
    public PartyCombatant combatant;

    private void Start()
    {
        combatant = GetComponent<PartyCombatant>();
    }

    public void Swap(GameObject screen)
    {
        for (int i = 1; i < 6; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        screen.SetActive(true);
    }
    public void OpenScreen(GameObject combatantIn)
    {
        transform.GetComponentInParent<Canvas>().enabled = true;
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

        combatant.combatant = combatantIn;
        combatant.name = combatantIn.GetComponent<PartyCombatant>().name;
        combatant.health = combatantIn.GetComponent<PartyCombatant>().health;
        combatant.maxHealth = combatantIn.GetComponent<PartyCombatant>().maxHealth;
        combatant.res = combatantIn.GetComponent<PartyCombatant>().res;

        GetComponentInChildren<ShowResistanceUI>().setRes();
        GetComponentInChildren<TextMeshProUGUI>().text = combatant.name + "(" + combatant.health + "/" + combatant.maxHealth + ")";
    }

    public void CloseScreen()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(Disable);
    }

    void Disable()
    {
        GetComponentInParent<Canvas>().enabled = false;
        FindObjectOfType<HoverStats>().ToggleCanvas(true);
        GetComponent<SwapScreen>().Swap(transform.GetChild(1).gameObject);
    }
}
