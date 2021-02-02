using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PartyCombatant combatant;
    private void Start()
    {
        combatant = GetComponentInParent<PartyCombatant>();
    }

    public void Update()
    {
        if (GetComponent<Slider>().value != combatant.combatantStats.getStat("HP") )
            SetHealth();
        if (GetComponent<Slider>().maxValue != combatant.combatantStats.getStat("Con"))
            SetMaxHealth();        
    }

    public void SetHealth()
    {
        GetComponent<Slider>().value = combatant.combatantStats.getStat("HP");
    }
    
    public void SetMaxHealth()
    {
        GetComponent<Slider>().maxValue = combatant.combatantStats.getStat("Con");
    }
}
