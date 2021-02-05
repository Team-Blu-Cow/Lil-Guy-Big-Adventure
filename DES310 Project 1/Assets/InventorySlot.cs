using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image icon;
    public Button removeBtn;
    public Canvas ConfirmCanvas;

    Item item;
    bool dragging;

    private void Update()
    {
        if (dragging)
        {
            icon.transform.position = (Input.mousePosition);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("InventoryItem") && item)
        {
            Debug.Log("Down Item");
            dragging = true;
        }
    }
    
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (item)
        {
            icon.transform.localPosition = new Vector3(0, 0, 0);
            dragging = false;

            GameObject eventGO = eventData.pointerCurrentRaycast.gameObject;

            if (!eventGO)
            {
                // Pop up to delete
                ConfirmCanvas.enabled = true;
                ConfirmCanvas.GetComponent<ConfirmDrop>().SetItem(item);
            }
            else if (eventGO.CompareTag("InventoryEquipt"))
            {
                Debug.Log("Up Equipt");
                // Equipt item
                eventGO.transform.parent.GetComponentInParent<PartyCombatant>().GetItems().combatantItems[eventGO.transform.parent.GetComponentInParent<PartyCombatant>().GetItems().combatantItems.Length] = item;
                Inventory.instance.items.Remove(item);
                transform.parent.GetComponentInParent<InventoryUI>().UpdateUI();
            }
            else
            {
                Debug.Log("Nothing happened");
            }
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = null;
        icon.enabled = true;
        removeBtn.interactable = true;
    }

    public void ClearItem()    
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeBtn.interactable = false;
    }
    
    public void RemoveButton()    
    {
        Inventory.instance.items.Remove(item);
    }
}
