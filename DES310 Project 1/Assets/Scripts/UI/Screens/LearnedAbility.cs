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
    public GameObject textGO;
    int abilitySelect = 0;

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        { 
            textGO.GetComponent<RectTransform>().position = (Mouse.current.position.ReadValue());
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("LearnedAbility") && gameObject.activeSelf)
        {
            dragging = true;
            textGO.GetComponent<LayoutElement>().ignoreLayout = true;

            if (eventData.pointerCurrentRaycast.gameObject.name.Contains("1"))
            {
                abilitySelect = 0;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name.Contains("2"))
            {
                abilitySelect = 1;
            }  
            else if (eventData.pointerCurrentRaycast.gameObject.name.Contains("3"))
            {
                abilitySelect = 2;
            }  
            else if (eventData.pointerCurrentRaycast.gameObject.name.Contains("4"))
            {
                abilitySelect = 3;
            }  
            else if (eventData.pointerCurrentRaycast.gameObject.name.Contains("5"))
            {
                abilitySelect = 4;
            } 
            else if (eventData.pointerCurrentRaycast.gameObject.name.Contains("6"))
            {
                abilitySelect = 5;
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

                if (eventGO.name.Contains("1"))
                {
                    Debug.Log(1);
                    combatant.abilitiesUsing[0] = combatant.abilitiesLearnt[abilitySelect];
                }
                else if (eventGO.name.Contains("2"))
                {
                    Debug.Log(2); 
                    combatant.abilitiesUsing[1] = combatant.abilitiesLearnt[abilitySelect];
                }
                else if (eventGO.name.Contains("3"))
                {
                    Debug.Log(3);
                    combatant.abilitiesUsing[2] = combatant.abilitiesLearnt[abilitySelect];
                }
                else if (eventGO.name.Contains("4"))
                {
                    Debug.Log(4);
                    combatant.abilitiesUsing[3] = combatant.abilitiesLearnt[abilitySelect];
                }
            }
        }

        dragging = false;
        textGO.GetComponent<LayoutElement>().ignoreLayout = false;
    }

    public void SetAbility(Combatant combatantIn, int index)
    {
        textGO.SetActive(true);
        if (combatantIn.abilitiesLearnt[index])
        {
            textGO.GetComponent<TextMeshProUGUI>().text = combatantIn.abilitiesLearnt[index].abilityName;
        }
        else
        {
            textGO.SetActive(false);
        }
    }
}
