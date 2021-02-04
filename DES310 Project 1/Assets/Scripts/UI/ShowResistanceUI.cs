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

        foreach (Aspects.Aspect aspect in combatant.combatant.GetComponent<Resistances>().vulnerabilities)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == aspect.ToString())
                {
                    transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        foreach (Aspects.Aspect aspect in combatant.combatant.GetComponent<Resistances>().resistances)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == aspect.ToString())
                {
                    transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
                }
            }
        }        
        
        foreach (Aspects.Aspect aspect in combatant.combatant.GetComponent<Resistances>().immunities)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == aspect.ToString())
                {
                    transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
}
