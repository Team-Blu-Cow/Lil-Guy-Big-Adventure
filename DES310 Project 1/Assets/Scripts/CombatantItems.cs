using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantItems : MonoBehaviour
{
    public Item[] combatantItems;
    public int currentItem = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem < 4)
            {
                currentItem++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if(currentItem > 0)
            {
                currentItem--;
            }
        }
    }
}
