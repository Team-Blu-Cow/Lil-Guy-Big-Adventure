using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoverToolTip : MonoBehaviour
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
                TMP.text= "Str: " + stats.getStat(Combatant_Stats.Strength) +   "   Dex: " + stats.getStat(Combatant_Stats.Dexterity) + "\n" + 
                "Mag: " + stats.getStat(Combatant_Stats.Magic)  +  "    Def: " + stats.getStat(Combatant_Stats.Defence) + "\n" + 
                "Con: " + stats.getStat(Combatant_Stats.Constitution)  +  "     Lck: " + stats.getStat(Combatant_Stats.Luck) + "\n" +
                "Spd: " + stats.getStat(Combatant_Stats.Speed) +  "   Init: " + stats.getStat(Combatant_Stats.Initiative) + "\n" + 
                "HP: " + stats.getStat(Combatant_Stats.HP);
            }
        }
    }

    public void SetResistance()
    {
        GetComponentInChildren<ShowResistanceUI>().SetRes(transform.parent.GetComponentInParent<PartyCombatant>());
    }
}
