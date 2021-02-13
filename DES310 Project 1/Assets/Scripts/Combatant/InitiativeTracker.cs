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

    public CombatButton[] moveButtons;
    //public CombatButton moveCancelButton;
    //public CombatButton moveConfirmButton;

    public CombatButton[] abilityButtons;
    //public CombatButton abilityOneButton;
    //public CombatButton abilityTwoButton;
    //public CombatButton abilityThreeButton;
    //public CombatButton abilityFourButton;

    public CombatButton[] itemButtons;
    //public CombatButton itemOneButton;
    //public CombatButton itemTwoButton;
    //public CombatButton itemThreeButton;
    //public CombatButton itemFourButton;
    //public CombatButton itemFiveButton;

    public CombatButton[] choiceButtons;
    //public CombatButton abilityConfirmButton;
    //public CombatButton itemConfirmButton;
    //public CombatButton waitConfirmButton;
    //public CombatButton backButton;

    bool selecting = false;
    public bool isAlly = false;
    public bool enemyChanging = false;

    public Vector3 combatantPos;
    public Vector3 combatantWorldPos;

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

                    //if (i != 0)
                    //{
                    //    Debug.Log((i-1) + " " + combatants[i-1].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) + "  " + i + " " + combatants[i].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) + "  " +  (i + 1) + " " + combatants[i + 1].GetComponent<Stats>().getStat(Combatant_Stats.Initiative));
                    //}
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
        
        // TODO Replace this with setting the current combatant being used as having an outline
        for (int i = 0; i < combatants.ToArray().Length; i++) // Loop for the amount of combatants
        {            
            if (combatants[i] != null) // Check for whether there is a combatant in the spot on the list
            {
                if (currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum) // If the current combatant number is the same as the combatant in the list's number then
                {
                    combatants[i].GetComponent<SpriteRenderer>().color = Color.red; // Change the colour to red to show it is their turn (TEMPORARY)
                    combatants[i].GetComponent<Combatant>().fighting = true; // Allow the combatant to fight
                }
                else // If not then
                {
                    combatants[i].GetComponent<SpriteRenderer>().color = Color.white; // Change the colour to white to show its not their turn
                    combatants[i].GetComponent<Combatant>().fighting = false; // Don't allow the combatant to fight
                }              
            }
        }


        if(getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Moved || getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Attacking)
        {

            combatantWorldPos = Camera.main.WorldToScreenPoint(getCurrentCombatant().transform.position);
            combatantPos = new Vector3(combatantWorldPos.x, combatantWorldPos.y, -1);

            if (getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Attacking && selecting == false)
            {
                int offsetY = 50;
                for(int i = 0; i < 3; i++)
                {
                    choiceButtons[i].activateButton(new Vector3(110, offsetY, 0), combatantPos);
                    offsetY -= 30;
                }
            }
            else if (getCurrentCombatant().GetComponent<Combatant>().combatantState == Combatant_State.Moved)
            {
                moveButtons[0].activateButton(new Vector3(110, -10, 0), combatantPos);
                moveButtons[1].activateButton(new Vector3(110, 20, 0), combatantPos);
            }
        }

        if(getCurrentCombatant().gameObject.tag == "Ally")
        {
            isAlly = true;
        }
        else
        {
            if (enemyChanging == false)
            {
                isAlly = false;
                StartCoroutine(enemyStuff());
            }
        }

        if (selecting == true)
        {
            for (int i = 0; i < 3; i++)
            {
                choiceButtons[i].deactivateButton();
            }
        }
    }


    public IEnumerator enemyStuff()
    {
        enemyChanging = true;
        Debug.Log("HI");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("HI AGAIN");
        ChangeCurrentCombatant();
        enemyChanging = false;
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

            for(int i = 0; i < combatants.ToArray().Length; i++)
            {
                combatants[i].GetComponent<Combatant>().combatantState = Combatant_State.Idle;
            }
        }
    }


    // TODO figure out the enigma that is these following for loops. I should just be able to call the combatant using the current combatant num variable however it doesn't work. :/
    // Ya dingus just use getCurrentCombatant()
    public void cancelMove()
    {
        for (int i = 0; i < combatants.ToArray().Length; i++)
        {
            if (currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum)
            {
                combatants[i].GetComponent<Combatant>().cancelMove();
                moveButtons[0].deactivateButton();
                moveButtons[1].deactivateButton();
                selecting = false;
            }
        }
    }

    public void confirmMove()
    {
        for (int i = 0; i < combatants.ToArray().Length; i++)
        {
            if (currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum)
            {
                combatants[i].GetComponent<Combatant>().confirmMove();
                moveButtons[0].deactivateButton();
                moveButtons[1].deactivateButton();
                selecting = false;
            }
        }
    }

    public void attackCombatant(int abilityNum)
    {
        for (int i = 0; i < combatants.ToArray().Length; i++)
        {
            if (currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum)
            {
                if (combatants[i].GetComponent<TestCombatSystem>().enemy != null)
                {
                    if (combatants[i].GetComponent<Combatant>().combatantState == Combatant_State.Attacking)
                    {
                        combatants[i].GetComponent<Combatant>().attackAbility(abilityNum);
                        for(int j = 0; j < 4; j++)
                        {
                            abilityButtons[j].deactivateButton();
                        }
                        choiceButtons[3].deactivateButton();
                    }
                }
                else
                {
                    Debug.Log("No Target has been added");
                }
            }
        }
    }

    public void useItem(int itemNum)
    {
        for (int i = 0; i < combatants.ToArray().Length; i++)
        {
            if (currentCombatantNum == combatants[i].GetComponent<Combatant>().combatantNum)
            {
                if (combatants[i].GetComponent<Combatant>().combatantState == Combatant_State.Attacking)
                {
                    combatants[i].GetComponent<Combatant>().UseItem(itemNum);
                    for(int j = 0; j < 5; j++)
                    {
                        itemButtons[i].deactivateButton();
                    }
                    choiceButtons[3].deactivateButton();
                }
            }
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

    public void activateAbilityButtons()
    {
        int offsetY = 110;

        for(int i = 0; i < 4; i++)
        {
            abilityButtons[i].activateButton(new Vector3(110, offsetY, 0), combatantPos);
            offsetY -= 30;
        }

        choiceButtons[3].activateButton(new Vector3(110, offsetY, 0), combatantPos);
        selecting = true;
    }

    public void activateItemButtonss()
    {
        int offsetY = 110;

        for (int i = 0; i < 5; i++)
        {
            itemButtons[i].activateButton(new Vector3(110, offsetY, 0), combatantPos);
            offsetY -= 30;
        }

        choiceButtons[3].activateButton(new Vector3(110, offsetY, 0), combatantPos);
        selecting = true;
    }

    public void useBackButton()
    {
        for(int i = 0; i < 4; i++)
        {
            abilityButtons[i].deactivateButton();
        }

        for(int i = 0; i < 5; i++)
        {
            itemButtons[i].deactivateButton();
        }

        choiceButtons[3].deactivateButton();
        selecting = false;
    }

    public void useWaitButton()
    {
        ChangeCurrentCombatant();
        getCurrentCombatant().GetComponent<Combatant>().combatantState = Combatant_State.Attacked;
        selecting = true;
    }
}
