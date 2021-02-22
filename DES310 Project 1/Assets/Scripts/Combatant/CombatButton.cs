using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatButton : MonoBehaviour
{

    public bool abilityButton = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().interactable = false;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }

    public void activateButton(Vector3 offset, Vector3 combatantPosition)
    {
        if (abilityButton == false)
        {
            GetComponent<Button>().interactable = true;
            GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            GetComponent<RectTransform>().anchoredPosition = combatantPosition + offset;
        }
        else if (abilityButton == true)
        {
            GetComponent<Button>().interactable = true;
            GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }
    }

    public void deactivateButton()
    {
        GetComponent<Button>().interactable = false;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false;
    }
}
