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
        GetComponentsInChildren<TextMeshProUGUI>()[0].text = transform.parent.GetComponentInParent<PartyCombatant>().Combatant.combatantName;
    }

    public void SetText()
    {
        Stats stats = transform.parent.GetComponentInParent<PartyCombatant>().CombatantStats;

        foreach (TextMeshProUGUI TMP in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (TMP.name == "Stats")
            {
                TMP.text= "Str: " + stats.GetStat(Combatant_Stats.Strength) +   "   Dex: " + stats.GetStat(Combatant_Stats.Dexterity) + "\n" + 
                "Mag: " + stats.GetStat(Combatant_Stats.Magic)  +  "    Def: " + stats.GetStat(Combatant_Stats.Defence) + "\n" + 
                "Con: " + stats.GetStat(Combatant_Stats.Constitution)  +  "     Lck: " + stats.GetStat(Combatant_Stats.Luck) + "\n" +
                "Spd: " + stats.GetStat(Combatant_Stats.Speed) +  "   Init: " + stats.GetStat(Combatant_Stats.Initiative) + "\n" + 
                "HP: " + stats.GetStat(Combatant_Stats.HP);
            }
        }
    }

    public void SetResistance()
    {
        GetComponentInChildren<ShowResistanceUI>().SetRes(transform.parent.GetComponentInParent<PartyCombatant>().Combatant);
    }
}
