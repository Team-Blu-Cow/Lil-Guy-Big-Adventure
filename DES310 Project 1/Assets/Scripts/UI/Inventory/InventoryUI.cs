using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Canvas FullInv;
    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;

        slots = GetComponentsInChildren<InventorySlot>();
    }
    public void UpdateUI()
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

    public void ToggleInventory()
    {
        FullInv.enabled = !FullInv.enabled;
    }
}
