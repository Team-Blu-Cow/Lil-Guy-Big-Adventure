using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCombatant : MonoBehaviour
{
    public GameObject combatant;

    public int health;
    public int maxHealth;
    public string name;
    public int[] res;

    public void UpdateScreen()
    {
        GetComponentInChildren<ShowResistanceUI>().setRes();
        GetComponentInChildren<RenameCombatantScreen>().Rename();
    }
}
