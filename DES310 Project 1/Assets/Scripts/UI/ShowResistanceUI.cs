using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShowResistanceUI : MonoBehaviour
{    
    public void SetRes(Combatant combatant)
    {
        int x = Enum.GetNames(typeof(Aspects.Aspect)).Length;
        
        for (int i = 0; i < x; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Image>().enabled = false;
            transform.GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
            transform.GetChild(i).GetChild(2).GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < x; i++)
        {
            if (combatant.resistances[i] <= 0)
            {
                transform.GetChild(i).GetChild(2).GetComponent<Image>().enabled = true;
            }
            else if (combatant.resistances[i] < 1)
            {
                transform.GetChild(i).GetChild(0).GetComponent<Image>().enabled =true;
            }
            else if (combatant.resistances[i] > 1)
            {
                transform.GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
            }
            
        }
    }
}

