using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCombatant : MonoBehaviour
{
    public GameObject combatant;
    Stats combatantStats;
    CombatantAbilities combatantAbilities;
    public string named;
    private void Start()
    {
        if (combatant.TryGetComponent<Stats>(out Stats stat))
            SetStats(stat);
        
        if (combatant.TryGetComponent<CombatantAbilities>(out CombatantAbilities abilitiea))
            SetAbilities(abilitiea);
    }

    public void SetAll(GameObject combatant, Stats stats, CombatantAbilities abilities, string names)
    {
        SetCombatant(combatant);
        SetStats(stats);
        SetAbilities(abilities);
        named = names;
    }

    public Stats GetStats()
    {
        return combatantStats;
    }
   public void SetStats(Stats stats)
    {
        combatantStats = stats;
    }

    public CombatantAbilities GetAbilities()
    {
        return combatantAbilities;
    }

    public void SetAbilities(CombatantAbilities abilities)
    {
        combatantAbilities = abilities;
    }
    
    public GameObject GetCombatant()
    {
        return combatant;
    }

    public void SetCombatant(GameObject combatantIn)
    {
        combatant = combatantIn;
    }
}
