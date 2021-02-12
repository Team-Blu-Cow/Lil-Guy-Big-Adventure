using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantAbilities : MonoBehaviour
{
    public Ability[] abilitiesLearnt;
    public Ability[] abilitiesUsing;
    public GameObject enemy;
    public Stats combatantStats;

    public int abilitySelect = 0;

    private void Start()
    {
        abilitiesUsing[0] = abilitiesLearnt[0];
        abilitiesUsing[1] = abilitiesLearnt[1];
        abilitiesUsing[2] = abilitiesLearnt[2];
        abilitiesUsing[3] = abilitiesLearnt[3];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilitySelect = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilitySelect = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            abilitySelect = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            abilitySelect = 3;
        }
    }    

    public void SetAbilityUsed(int abilityNum)
    {
        abilitiesUsing[abilitySelect] = abilitiesLearnt[abilityNum];
    }
}
