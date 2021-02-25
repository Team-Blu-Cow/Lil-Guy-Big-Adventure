using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
            icon.transform.position = (Mouse.current.position.ReadValue());
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.CompareTag("InventoryItem") && item)
        {
            dragging = true;
        }       
    }
    
    public void OnPointerUp(PointerEventData eventData)
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
                int index = eventGO.transform.parent.name[0]-48;
                PartyCombatant combatant = eventGO.transform.parent.parent.GetComponentInParent<PartyCombatant>();
                // Equipt item
                if (combatant.Combatant.combatantItems[index] != null)
                {
                    Inventory.instance.items.Add(combatant.Combatant.combatantItems[index]);
                }

                combatant.Combatant.combatantItems[index] = item;
                eventGO.GetComponentsInChildren<Image>()[1].sprite = item.itemImage;
                eventGO.GetComponentsInChildren<Image>()[1].enabled = true;

                item.transform.parent = combatant.CombatantGO.transform;
                item.gameObject.SetActive(false);

                // Removes from inventory
                Inventory.instance.items.Remove(item);
                transform.parent.parent.GetComponentInParent<InventoryUI>().UpdateUI();
            }
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.itemImage;
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

    public Item GetItem()
    {
        return item;
    }
}
