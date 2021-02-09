using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Canvas fullInventory;
    Inventory inventory;

    InventorySlot[] slots;

    private void Awake()
    {
        inventory = Inventory.instance;

        slots = GetComponentsInChildren<InventorySlot>();
    }

    // Start is called before the first frame update
    void Start()
    {        
        fullInventory.enabled = false;
    }

    public void UpdateUI()
    {
        if (fullInventory.enabled)
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
        fullInventory.enabled = toggle;
        if (fullInventory.enabled)
        {
            UpdateUI();
        }            
    }
    
    public void FlipInventory()
    {
        fullInventory.enabled = !fullInventory.enabled;
        if (fullInventory.enabled)
        {
            UpdateUI();
        }            
    }
}
