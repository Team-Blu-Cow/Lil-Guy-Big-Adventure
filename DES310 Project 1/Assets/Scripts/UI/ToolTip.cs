using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventorySlot slot;
    public Canvas toolTip;

    private void Awake()
    {
        slot = GetComponent<InventorySlot>();
    }

    private void Update()
    {
        FollowCursor();
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
        int padding = 15;

        if (toolTip.enabled)
        {
            Vector3 newPos = Mouse.current.position.ReadValue() + new Vector2(padding,padding);

            if (newPos.x + toolTip.GetComponentInChildren<RectTransform>().rect.width > Screen.width - padding)
            {
                newPos.x = Screen.width - toolTip.GetComponentInChildren<RectTransform>().rect.width - padding;
            }

            if (newPos.y + toolTip.GetComponentInChildren<RectTransform>().rect.height > Screen.height - padding)
            {
                newPos.y = Screen.height - toolTip.GetComponentInChildren<RectTransform>().rect.height - padding;
            }
            
            if (newPos.x < padding)
            {
                newPos.x = padding;
            }
            
            if (newPos.y < padding)
            {
                newPos.y = padding;
            }

            toolTip.transform.position = newPos;
        }

        
    }
}
