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
        if (abilityButton)
            transform.GetChild(0).gameObject.SetActive(false);
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
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void deactivateButton()
    {
        GetComponent<Button>().interactable = false;
        GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        if (abilityButton)
            transform.GetChild(0).gameObject.SetActive(false);
    }
}
