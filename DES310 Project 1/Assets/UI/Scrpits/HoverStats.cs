using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class HoverStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject currentHover;
    float hoverTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHover)
        {           
            if (hoverTime > 0.5f && !currentHover.activeInHierarchy)
            {
                ShowStats();
            }
            else if (hoverTime < 0.5f)
            {
                hoverTime += Time.deltaTime;
            }
        }

       // currentHover.GetComponentInParent<PartyCombatant>(); // This gets what combantnt is int his party slot
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("UIPartyMember"))
        {
            currentHover = eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject;            
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(currentHover, new Vector3(0, 0, 0), 0.5f).setOnComplete(HideStats);
        hoverTime = 0;
    }

    void ShowStats()
    {              
        currentHover.SetActive(true);
        LeanTween.scale(currentHover, new Vector3(1, 1, 1), 0.5f);
    }

    void HideStats()
    {
        if (currentHover)
            currentHover.SetActive(false);

        currentHover = null;
    }
}
