using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCombatant : MonoBehaviour
{
    public GameObject CombatantGO { get; set; }
    public Stats CombatantStats { get; set; }
    public Combatant Combatant { get; set; }

    private void Awake()
    {
        if (CombatantGO == null)
            CombatantGO = transform.gameObject;  
    }

    private void Start()
    {
        if (CombatantGO.TryGetComponent<Stats>(out Stats stat))
            CombatantStats = stat;
        
        if (CombatantGO.TryGetComponent<Combatant>(out Combatant combatantScript))
            Combatant = combatantScript;
    }

    public void SetAll(GameObject combatantGO)
    {
        CombatantGO = combatantGO;
        CombatantStats = combatantGO.GetComponent<Stats>();
        Combatant  = combatantGO.GetComponent<Combatant>();
    }
}
