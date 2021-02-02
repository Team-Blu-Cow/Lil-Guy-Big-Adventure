using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RenameCombatantScreen : MonoBehaviour
{
    public void Rename()
    {
        PartyCombatant combatant = GetComponentInParent<PartyCombatant>();
        GetComponent<TextMeshProUGUI>().text = combatant.name + "(" + combatant.health + "/" + combatant.maxHealth + ")" ;
    }
}
