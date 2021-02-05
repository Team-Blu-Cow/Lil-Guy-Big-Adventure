using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventorySlot slot;
    public Canvas toolTip;

    private void Awake()
    {
        slot = GetComponent<InventorySlot>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        toolTip.transform.position = Input.mousePosition;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("InventoryItem") && slot.GetItem())
        {
            toolTip.enabled = true;
            SetHeader();
            SetText();
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        toolTip.enabled = false;
    }

    public void SetHeader()
    {
        toolTip.GetComponentsInChildren<TextMeshProUGUI>()[0].text = slot.GetItem().name;
    }

    public void SetText()
    {
        toolTip.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Str: " + slot.GetItem().itemType + "\n" +
        "Str: " + slot.GetItem().itemDuration + "\n" +
        "Str: " + slot.GetItem().itemPower + "\n" +
        "Str: " + slot.GetItem().statBoost + "\n";
    }

    void FollowCursor()
    {

    }
}
