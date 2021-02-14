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

    public void PickUp(Vector3 playerPos)
    {
        bool pickedUp = false;
        AStarGrid grid = GameObject.FindObjectOfType<AStarGrid>();
        AStarNode node = grid.WorldToNode(transform.position);

        foreach (AStarNode neighbor in grid.GetNeighbors(node))
        {
            if (neighbor.gridPosition == grid.WorldToNode(playerPos).gridPosition) //if it is a treasure
            {
                //Collect
                pickedUp = true;
                Inventory.instance.Add(this);
                gameObject.SetActive(false);
                Debug.Log("Picked Up");
            }
        }

        if (!pickedUp)
        {
            //Show cant pickup
            Debug.Log("Cant pickup");
        }
    }
}
