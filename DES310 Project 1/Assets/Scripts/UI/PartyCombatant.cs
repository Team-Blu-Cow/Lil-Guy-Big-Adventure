using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCombatant : MonoBehaviour
{
    [HideInInspector] public GameObject combatantGO;
    Stats combatantStats;
    Combatant combatant;
    public string named;

    private void Awake()
    {
        if (combatantGO == null)
            combatantGO = transform.gameObject;  
    }

    private void Start()
    {
        if (combatantGO.TryGetComponent<Stats>(out Stats stat))
            SetStats(stat);
        
        if (combatantGO.TryGetComponent<Combatant>(out Combatant combatant))
            SetCombatant(combatant);
    }

    public void SetAll(GameObject combatantGO, Stats stats, Combatant combtantScript, string names)
    {
        SetCombatantGO(combatantGO);
        SetStats(stats);
        SetCombatant(combtantScript);
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
    
    public Combatant GetCombatant()
    {
        return combatant;
    }
   public void SetCombatant(Combatant combatantScrpit)
    {
        combatant = combatantScrpit;
    }
    
    public GameObject GetCombatantGO()
    {
        return combatantGO;
    }

    public void SetCombatantGO(GameObject combatantIn)
    {
        combatantGO = combatantIn;
    }
}
