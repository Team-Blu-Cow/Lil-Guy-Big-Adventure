using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Canvas inventoryCanvas;
    Inventory inventory;

    InventorySlot[] slots;

    private void Awake()
    {
        inventory = Inventory.instance;

        slots = GetComponentsInChildren<InventorySlot>();
        inventoryCanvas = transform.GetChild(1).GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inventoryCanvas.enabled = false;
    }

    public void UpdateUI()
    {
        if (inventoryCanvas.enabled)
        {
            for (int i = 0; i < inventory.invSpace; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                }
                else
                {
                    slots[i].ClearItem();
                }
            }
        }
    }

    public void ToggleInventory(bool toggle)
    {
        inventoryCanvas.enabled = toggle;
        if (inventoryCanvas.enabled)
        {
            UpdateUI();
        }            
    }
    
    public void FlipInventory()
    { 
        inventoryCanvas.enabled = !inventoryCanvas.enabled;

        if (inventoryCanvas.enabled)
        {
            UpdateUI();
        }            
    }
}
