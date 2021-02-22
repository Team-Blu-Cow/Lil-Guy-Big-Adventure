using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public void Update()
    {
        if (GetComponent<Slider>().value != GetComponentInParent<PartyCombatant>().GetStats().GetStat(Combatant_Stats.HP))
            SetHealth();
        if (GetComponent<Slider>().maxValue != GetComponentInParent<PartyCombatant>().GetStats().GetStat(Combatant_Stats.Constitution))
            SetMaxHealth();        
    }

    public void SetHealth()
    {
        GetComponent<Slider>().value = GetComponentInParent<PartyCombatant>().GetStats().GetStat(Combatant_Stats.HP);
    }
    
    public void SetMaxHealth()
    {
        GetComponent<Slider>().maxValue = GetComponentInParent<PartyCombatant>().GetStats().GetStat(Combatant_Stats.Constitution);
    }
}
