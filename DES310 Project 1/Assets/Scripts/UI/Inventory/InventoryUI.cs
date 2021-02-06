using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject fullInventory;
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
        fullInventory.SetActive(false);
    }

    public void UpdateUI()
    {
        if (fullInventory.activeSelf)
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
        fullInventory.SetActive(toggle);
        if (fullInventory.activeSelf)
        {
            UpdateUI();
        }            
    }
}
