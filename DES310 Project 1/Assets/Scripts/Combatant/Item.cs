using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum item_type
{
    Poison = 0,
    Heal = 1,
    Buff = 2
}

public enum item_duration
{
    Passive = 0,
    Active = 1,
    ThreeTurns = 2
}

public enum stat_boost
{
    None = 0,
    Strength = 1,
    Magic = 2,
    Dexterity = 3,
    Defence = 4,
    Constitution = 5,
    Luck = 6,
    Speed = 7,
    Initiative = 8
}

public class Item : MonoBehaviour
{
    public string itemName;
    public item_type itemType;
    public item_duration itemDuration;
    public stat_boost statBoost;
    public int itemPower;
    public Sprite itemImage;
    public bool pickedUp;

    public void PickUp(Vector3 playerPos)
    {
        IsoGrid grid = GameObject.FindObjectOfType<IsoGrid>();
        IsoNode node = grid.WorldToNode(transform.position);

        foreach (IsoNode neighbor in grid.GetNeighbors(node))
        {
            if (neighbor.gridPosition == grid.WorldToNode(playerPos).gridPosition && !pickedUp) //if it is a treasure
            {
                //Collect
                Inventory.instance.Add(this);
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Items/open_chest");
                pickedUp = true;
                DamagePopup.Create("Found a "+itemName, transform.position);
                // Debug.Log("Picked Up");
            }
        }
    }
}
