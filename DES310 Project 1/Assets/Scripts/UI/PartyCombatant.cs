using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCombatant : MonoBehaviour
{
    public GameObject combatant;
    Stats combatantStats;
    CombatantAbilities combatantAbilities;
    CombatantItems combatantItems;
    public string named;

    private void Awake()
    {
        if (combatant == null)
            combatant = transform.gameObject;        
    }

    private void Start()
    {
        if (combatant.TryGetComponent<Stats>(out Stats stat))
            SetStats(stat);
        
        if (combatant.TryGetComponent<CombatantAbilities>(out CombatantAbilities abilitiea))
            SetAbilities(abilitiea);
        
        if (combatant.TryGetComponent<CombatantItems>(out CombatantItems items))
            SetItems(items);
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
    
    public void SetItems(CombatantItems items)
    {
        combatantItems = items;
    }
    
    public CombatantItems GetItems()
    {
       return combatantItems;
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
