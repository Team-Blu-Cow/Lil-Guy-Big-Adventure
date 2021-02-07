using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class LeanedAbility : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    bool dragging = false;
    public GameObject textGO;

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        { 
            textGO.GetComponent<RectTransform>().anchoredPosition = (Mouse.current.position.ReadValue());
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("LearnedAbility") && gameObject.activeSelf)
        {
            dragging = true;
            textGO.GetComponent<LayoutElement>().ignoreLayout = true;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Up");

        if (gameObject.activeSelf)
        {
            dragging = false;
            textGO.GetComponent<LayoutElement>().ignoreLayout = false;


            GameObject eventGO = eventData.pointerCurrentRaycast.gameObject;

            //if (!eventGO)
            //{
            //}
            //else if (eventGO.CompareTag("Abilty"))
            //{
            //    // set ability
            //    combatant.abilitiesUsing[0] = combatant.abilitiesLearnt[0];

            //}
        }
    }

    public void SetAbility(Combatant combatant, int index)
    {
        if (combatant.abilitiesLearnt[index])
        {
            textGO.GetComponent<TextMeshProUGUI>().text = combatant.abilitiesLearnt[index].abilityName;
        }
    }
}
