using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowResistanceUI : MonoBehaviour
{    
    public void setRes(PartyCombatant combatant)
    {        
        for (int i = 0; i < transform.childCount; i++)
        {
            switch (combatant.res[i])
            {
                case 0: 
                    transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                    break;
                case 1:
                    transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    break;
                default:
                    transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    break;
            }            
        }
    }
}
