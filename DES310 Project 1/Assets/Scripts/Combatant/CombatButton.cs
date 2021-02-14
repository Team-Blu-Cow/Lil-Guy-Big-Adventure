using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatButton : MonoBehaviour
{

    public bool abilityButton = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void activateButton(Vector3 offset, Vector3 combatantPosition)
    {
        if (abilityButton == false)
        {
            gameObject.SetActive(true);
            GetComponent<RectTransform>().anchoredPosition = combatantPosition + offset;
        }
        else if (abilityButton == true)
        {
            gameObject.SetActive(true);
            GetComponent<RectTransform>().anchoredPosition = offset;
        }
    }

    public void deactivateButton()
    {
        gameObject.SetActive(false);
    }
}
