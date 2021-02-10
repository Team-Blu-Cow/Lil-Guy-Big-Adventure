using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeTracker : MonoBehaviour
{
    public List<GameObject> combatants;
    public InputManager controls;
    int combatantNum = 0;
    bool sorted = false;
    int currentCombatantNum = 0;
    public List<int> combatantInits;

    Color color;
    Color white;

    private void Awake()
    {
        controls = new InputManager();
        controls.Keyboard.ChangePartyMember.started += ctx => ChangeCurrentCombatant();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        white.r = 255;
        white.g = 255;
        white.b = 255;
        white.a = 255;
        color.r = 255;
        color.g = 0;
        color.b = 0;
        color.a = 255;
    }

    // Update is called once per frame
    void Update()
    {
        // The following code is a bit of a mess, I am sorry :(
        if (sorted == false) // If the combatant's initiatives have not been sorted
        {
            sorted = true; // Set the sort to true as we will sort them in this if statement

            for (int i = 0; i < combatants.ToArray().Length; i++) // Start a for loop which get's each of the combatant's initiatives
            {
                if (combatants[i] != null) // Check for whether there is a combatant in the spot on the list
                {
                    Stats combatantStats = combatants[i].GetComponent<Stats>(); // Create temp variable for combatant's stats
                    combatantInits.Add(combatantStats.getStat(Combatant_Stats.Initiative)); // Add the combatant's initiative to the list
                }
            }
            combatantInits.Sort(); // Sort the list of combatant initiatives

            // This needs to be after the sort
            for (int i = 0; i < combatants.ToArray().Length; i++) // Loop for the number of combatant's
            {

                if (i != (combatants.ToArray().Length - 1)) // If we are not at the end of the list (as in there is still a value to be read next after the current one we are on
                {
                    if (combatantInits[i] == combatantInits[i + 1]) // If two initiatives are the same then
                    {
                        Debug.Log("Happened");
                        for (int k = 0; k < combatants.ToArray().Length; k++) // Start another for loop for the amount of combatants
                        {
                            if (combatants[k].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) == combatantInits[i]) // Find the first combatant which has the same initiative
                            {                               
                                Debug.Log("Happened 2");
                                combatantInits[i] = combatantInits[i] + 1; // Change the initiative in the list to be up 1 so that the initiatives don't match
                                combatants[k].GetComponent<Stats>().setStat(Combatant_Stats.Initiative, (combatants[k].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) + 1)); // Add 1 to the combatant's initiative so that the combatant's and the list of initiatives match
                                Debug.Log(k + " INIT: " + combatants[k].GetComponent<Stats>().getStat(Combatant_Stats.Initiative));
                                break; // Break out of the for loop before we find the other combatant that has the same initiative
                            }
                        }
                    }

                    if (i != 0)
                    {
                        Debug.Log((i-1) + " " + combatants[i-1].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) + "  " + i + " " + combatants[i].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) + "  " +  (i + 1) + " " + combatants[i + 1].GetComponent<Stats>().getStat(Combatant_Stats.Initiative));
                    }
                }

               

                for (int j = 0; j < combatantInits.ToArray().Length; j++) // Start a for loop for the amount of combatants
                {
                    if (combatants[i] != null) // Check for whether there is a combatant in the spot on the list
                    {                       
                        if (combatants[i].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) == combatantInits[j]) // If the combatant's and the list's initiatives match up then
                        {
                            combatants[i].GetComponent<Combatant>().combatantNum = (combatants.ToArray().Length - 1) - j; // Set the combatant's number to be the increment of the for loop. This essentially sets their position in the initiative for combat
                            break; // Break the for loop. Just in case?
                        }
                    }
                }
                
            }
        }
        

        for (int i = 0; i < combatants.ToArray().Length; i++) // Loop for the amount of combatants
        {            
            if (combatants[i] != null) // Check for whether there is a combatant in the spot on the list
            {
                if (currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum) // If the current combatant number is the same as the combatant in the list's number then
                {
                    combatants[i].GetComponent<SpriteRenderer>().color = color; // Change the colour to red to show it is their turn (TEMPORARY)
                    combatants[i].GetComponent<Combatant>().fighting = true; // Allow the combatant to fight
                }
                else // If not then
                {
                    combatants[i].GetComponent<SpriteRenderer>().color = white; // Change the colour to white to show its not their turn
                    combatants[i].GetComponent<Combatant>().fighting = false; // Don't allow the combatant to fight
                }
            }
        }
    }

    public void AddCombatant(GameObject combatant)
    {
        combatants.Add(combatant);
        combatantNum++;
    }

    public void ChangeCurrentCombatant()
    {
        currentCombatantNum++;
        if(currentCombatantNum == combatants.ToArray().Length)
        {
            currentCombatantNum = 0;
        }
    }

    public GameObject getCurrentCombatant()
    {
        for(int i = 0; i < combatants.ToArray().Length; i++)
        {
            if(currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum)
            {
                return combatants[i];
            }           
        }

        return null;
    }
}
