using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmDrop : MonoBehaviour
{
    Item item;

    public void SetItem(Item newItem)
    {
        item = newItem;
    }

    public void Keep()
    {
        item = null;
        GetComponent<Canvas>().enabled = false;
    }

    public void Drop()
    {
        Inventory.instance.items.Remove(item);
        GetComponent<Canvas>().enabled = false;
        item = null;
    }
}
