using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public void Update()
    {
        if (GetComponent<Slider>().value != GetComponentInParent<PartyCombatant>().health)
            SetHealth();
        if (GetComponent<Slider>().maxValue != GetComponentInParent<PartyCombatant>().maxHealth)
            SetMaxHealth();        
    }

    public void SetHealth()
    {
        GetComponent<Slider>().value = GetComponentInParent<PartyCombatant>().health;
    }
    
    public void SetMaxHealth()
    {
        GetComponent<Slider>().maxValue = GetComponentInParent<PartyCombatant>().maxHealth;
    }
}
