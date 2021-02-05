using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum quirk_stat
{
    None = 0,
    Strength = 1,
    Magic = 2,
    Dexterity = 3,
    Defence = 4,
    Constitution = 5,
    Luck = 6,
    Speed = 7,
    Initiative = 8
}

public class Quirks : MonoBehaviour
{
    public string quirkName;
    public int quirkPower;
    public Aspects.Aspect quirkAspect;
    public quirk_stat statBoost;
    public float quirkResistance;

}
