using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowResistanceUI : MonoBehaviour
{    
    public void SetRes(PartyCombatant combatant)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (combatant.GetCombatant().resistances[i] < 1)
            {
                transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
            else if (combatant.GetCombatant().resistances[i] > 1)
            {
                transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
            else if (combatant.GetCombatant().resistances[i] <= 0)
            {
                transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            }
        }
    }
}

