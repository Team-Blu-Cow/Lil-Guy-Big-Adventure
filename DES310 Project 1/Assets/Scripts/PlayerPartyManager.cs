using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPartyManager : MonoBehaviour
{
    public GameObject[] party = new GameObject[4];
    public int totalAreasVisited = 1;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Remove()
    {
        foreach (GameObject item in GetComponentsInChildren<GameObject>())
        {
            Destroy(item);
        }
    }

    public void AddCombatant(GameObject combatant)
    {
        bool added = false;

        for (int i = 0; i < party.Length; i++)
        {
            if (party[i] == combatant)
            {
                Debug.Log("Combatant in party");
                break;
            }

            if (party[i] == null)
            {
                party[i] = combatant;
                combatant.tag = "Ally";
                combatant.gameObject.SetActive(true);                
                combatant.transform.parent = transform;
                combatant.GetComponent<Stats>().SetModStat(Combatant_Stats.HP, combatant.GetComponent<Stats>().GetFinalStat(Combatant_Stats.Constitution));
                FindObjectOfType<MapGeneration>().placedEnemies.Remove(combatant);
                added = true;
                break;
            }
        }

        if (added)
        {
            Debug.Log("Added combatant to party");
        }
        else
        {
            Debug.Log("Combatant not added to party");

        }
    }
}
