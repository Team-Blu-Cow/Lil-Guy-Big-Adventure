using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class LearnedAbility : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    bool dragging = false;
    int abilitySelect = 0;

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        { 
            GetComponentsInChildren<RectTransform>()[2].position = (Mouse.current.position.ReadValue());
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("LearnedAbility") && gameObject.activeSelf)
        {
            dragging = true;
            GetComponentInChildren<LayoutElement>().ignoreLayout = true;

            for (int i = 0; i < 10; i++)
            {
                if (eventData.pointerCurrentRaycast.gameObject.name.Contains((i+1).ToString()))
                {
                    abilitySelect = i;
                }
            }
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        GameObject eventGO = eventData.pointerCurrentRaycast.gameObject;

        if (eventGO)
        {
            if (eventGO.CompareTag("UsingAbility") && gameObject.activeSelf)
            {
                Combatant combatant = eventGO.transform.parent.GetComponentInParent<PartyCombatant>().GetCombatant();

                for (int i = 0; i < 10; i++)
                {
                    if (eventGO.name.Contains((i + 1).ToString()))
                    {
                        combatant.abilitiesUsing[i] = combatant.abilitiesLearnt[abilitySelect];
                    }
                }
                ScreenManager.screenManager.GetCombatantScreen().SetAblities(combatant);
            }
        }

        dragging = false;
        GetComponentInChildren<LayoutElement>().ignoreLayout = false;
    }

    public void SetAbility(Combatant combatantIn, int index)
    {
        gameObject.SetActive(true);
        if (combatantIn.abilitiesLearnt[index])
        {
            GetComponentsInChildren<TextMeshProUGUI>()[1].text = combatantIn.abilitiesLearnt[index].abilityName;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
