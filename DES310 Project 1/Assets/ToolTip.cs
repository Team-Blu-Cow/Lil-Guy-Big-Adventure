using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public int characterWrapLimit;

    private void Start()
    {
       
    }

    private void Update()
    {
        SetHeader();
        SetText();
        SetResistance();
            
        //int headerLength = GetComponentsInChildren<TextMeshProUGUI>()[0].text.Length ;
        //int contentLength = GetComponentsInChildren<TextMeshProUGUI>()[1].text.Length/4 ;

        //GetComponent<LayoutElement>().enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    public void SetHeader()
    {
        GetComponentsInChildren<TextMeshProUGUI>()[0].text = transform.parent.GetComponentInParent<PartyCombatant>().named;
    }

    public void SetText()
    {
        Stats stats = transform.parent.GetComponentInParent<PartyCombatant>().GetStats();

        foreach (TextMeshProUGUI TMP in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (TMP.name == "Stats")
            {
                TMP.text= "Str: " + stats.getStat("Str") +   "   Dex: " + stats.getStat("Dex") + "\n" + 
                "Mag: " + stats.getStat("Mag")  +  "    Def: " + stats.getStat("Def") + "\n" + 
                "Con: " + stats.getStat("Con")  +  "     Lck: " + stats.getStat("Luck") + "\n" +
                "Spd: " + stats.getStat("Speed")+  "   Init: " + stats.getStat("Init") + "\n" + 
                "HP: " + stats.getStat("HP");
            }
        }
    }

    public void SetResistance()
    {
        GetComponentInChildren<ShowResistanceUI>().SetRes(transform.parent.GetComponentInParent<PartyCombatant>());
    }
}
