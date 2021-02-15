using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState {SLEEPING, START, IN_BATTLE, FINISHED }
public enum CombatantState { START, MOVE, ACTION, END }
public class BattleManager : MonoBehaviour
{
    public GameObject[] enemyCombatants;
    [SerializeField] private PlayerPartyManager playerParty;

    [SerializeField] List<GameObject> combatants;
    [SerializeField] Queue<GameObject> battleQueue;
    public GameObject currentCombatant = null;

    public bool recievedMoveCommand = false;
    public bool receivedActionCommand = false;

    public Vector3 targetPos;
    public Vector2 uiPos;

    public BattleState battleState;
    public CombatantState combatantState;

    //public InitiativeTracker initTracker;
    [SerializeField] private GridHighLighter gridHighLighter;
    [SerializeField] private CombatUI combatUI;

    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.SLEEPING;
        combatantState = CombatantState.START;
    }

    private void Update()
    {
        switch (battleState)
        {
            case BattleState.SLEEPING:
                battleState = BattleState.START;
                break;

            case BattleState.START:
                StartBattle();
                break;

            case BattleState.IN_BATTLE:
                InBattle();
                break;
        }

    }
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    // DATA STRUCTURING METHODS                                                                                                                   //
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    void SetBattleQueue()
    {
        battleQueue = new Queue<GameObject>();

        foreach (var combatant in combatants)
        {
            battleQueue.Enqueue(combatant);
        }

    }

    void CycleQueue()
    {
        currentCombatant = battleQueue.Dequeue();
        battleQueue.Enqueue(currentCombatant);
    }

    public void SortBattleInitiative()
    {
        bool swapped = true;

        while (swapped)
        {
            swapped = false;

            for (int j = 1; j < combatants.Count; j++)
            {
                if (combatants[j - 1].GetComponent<Stats>().getStat(Combatant_Stats.Initiative) < combatants[j].GetComponent<Stats>().getStat(Combatant_Stats.Initiative))
                {
                    Swap(combatants, j - 1, j);
                    swapped = true;
                }
            }
        }
    }

    public void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    // INPUT METHODS                                                                                                                              //
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public void RecieveMouseClick(Vector3 mousePos)
    {
        switch (battleState)
        {
            case BattleState.IN_BATTLE:
                switch (combatantState)
                {
                    case CombatantState.MOVE:
                        RecieveMove(mousePos);
                        break;

                    case CombatantState.ACTION:
                        break;
                }
                break;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    // BATTLE STATE METHODS                                                                                                                       //
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    void StartBattle()
    {
        combatants = new List<GameObject>();

        combatants.AddRange(enemyCombatants);
        foreach (var member in playerParty.party)
        {
            if (member != null)
                combatants.Add(member);
        }

        SortBattleInitiative();
        SetBattleQueue();

        //currentCombatantIndex = 0;

        battleState = BattleState.IN_BATTLE;
    }

    void InBattle()
    {
        switch (combatantState)
        {
            case CombatantState.START:
                StartTurn();
                break;

            case CombatantState.MOVE:
                TurnMovePhase();
                break;

            case CombatantState.ACTION:
                TurnActionPhase();
                break;

            case CombatantState.END:
                EndTurn();
                break;
        }

    }

    

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    // COMBATANT STATE METHODS                                                                                                                    //
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    // Start Turn Phase ***************************************************************************
    void StartTurn()
    {
        CycleQueue();
        currentCombatant.GetComponent<PathFindingUnit>().SetSelectableTiles(currentCombatant.GetComponent<Stats>().getStat(Combatant_Stats.Speed));

        combatantState = CombatantState.MOVE;

        Debug.Log("Start Turn");

        if (currentCombatant.tag == "Ally")
        {
            Debug.Log("Player Move Phase");
        }
        else if (currentCombatant.tag == "Enemy")
        {
            Debug.Log("Enemy Move Phase");
            StartCoroutine(AIMove());
        }
    }

    // Move Phase *********************************************************************************
    void TurnMovePhase()
    {
        // do move stuff? not sure if this function is actually required. for AI maybe?

        // grid highlighter draw arrow
    }

    IEnumerator AIMove()
    {
        yield return new WaitForSeconds(5.0f);
        combatantState = CombatantState.ACTION;
    }

    public void RecieveMove(Vector3 mousePos)
    {
        if (!recievedMoveCommand)
        {
	        if (currentCombatant.tag == "Ally" && gridHighLighter.IsTileSelectable(gridHighLighter.grid.WorldToNode(mousePos)))
	        {
                uiPos = combatUI.WorldToCanvasSpace(mousePos);

                targetPos = mousePos;
	            combatUI.activateMoveButtons(uiPos);
	            recievedMoveCommand = true;
	        }
        }
    }

    public void OnConfirmMove()
    {
        combatUI.deactivateMoveButtons();
        recievedMoveCommand = false;
        currentCombatant.GetComponent<Combatant>().oldPosition = currentCombatant.GetComponent<Transform>().position;
        currentCombatant.GetComponent<PathFindingUnit>().RequestPath(new Vector2(targetPos.x, targetPos.y));

        gridHighLighter.ClearSelectableTiles();
        combatantState = CombatantState.ACTION;
        combatUI.activateChoiceButtons(uiPos);
    }

    public void OnCancelMove()
    {
        combatUI.deactivateMoveButtons();
        recievedMoveCommand = false;
    }

    // Action Phase *******************************************************************************
    void TurnActionPhase()
    {
        // same as move phase function, unsure if this is needed

        if (currentCombatant.tag == "Enemy")
            combatantState = CombatantState.END;
    }

    IEnumerator AIAction()
    {
        yield return new WaitForSeconds(0.5f);
        combatantState = CombatantState.END;
    }

    public void OnSelectAbility()
    {
        combatUI.activateAbilityButtons();
    }

    public void OnBackButton()
    {
        combatUI.useBackButton();
    }

    public void OnWait()
    {
        combatUI.deactivateAbilityButtons();
        combatUI.deactivateItemButtons();
        combatUI.deactivateChoiceButtons();
        combatantState = CombatantState.END;
    }


    // End Phase  *********************************************************************************
    void EndTurn()
    {
        // check if player has won or lost

        combatantState = CombatantState.START;
        Debug.Log("End Turn");
    }

}
