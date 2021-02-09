using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ability_type
{
    Damage = 0,
    Heal = 1,
    Buff = 2
}

public enum stat_used
{
    Strength = 0,
    Magic = 1,
    Dexterity = 2,
    Defence = 3, 
    Constitution = 4,
    Luck = 5,
    Speed = 6,
    Initiative = 7
}

public class Ability : MonoBehaviour
{

    public string abilityName;
    public ability_type abilityType;
    public stat_used statUsed;
    public int abilityPower;
    public int abilityRange;
    public int abilityArea;
    public Aspects.Aspect abilityAspect;    
}