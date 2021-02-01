using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class HoverStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject combatant;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("UIPartyMember"))
        {
            ShowStats(eventData.pointerCurrentRaycast.gameObject);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        HideStats();       
    }

    void ShowStats(GameObject mouseOver)
    {
        mouseOver.transform.GetChild(0).gameObject.SetActive(true);
    }

    void HideStats()
    {

    }
}
