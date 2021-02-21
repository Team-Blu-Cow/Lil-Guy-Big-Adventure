using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BattleState { SLEEPING, START, IN_BATTLE, FINISHED }
public enum CombatantState { START, MOVE, ACTION, END }
public enum ActionState { NOT_SELECTED, WAIT, ABILITY, ITEM, FINISHED }
public class BattleManager : MonoBehaviour
{
    public List<GameObject> enemyCombatants;
    [SerializeField] private PlayerPartyManager playerParty;

    [SerializeField] List<GameObject> combatants;
    [SerializeField] Queue<GameObject> battleQueue;
    public GameObject currentCombatant = null;
    public GameObject selectedCombatant = null;

    private bool currentlyMoving = false;
    private bool recievedMoveCommand = false;
    private bool receivedActionCommand = false;
    private int selectedAbility = 0;
    private int selectedItem;

    private Vector3 abilityTargetPos;
    [HideInInspector] public bool animEffectComplete = false;
	
    public Vector3 targetPos;
    public Vector3 cursorPos;
    public Vector2 uiPos;

    [SerializeField] private BattleState battleState;
    public BattleState BattleState { get { return BattleState; } set { battleState = value; } }
    [SerializeField] private CombatantState combatantState; 
    public CombatantState CombatantState { get { return combatantState; } set { combatantState = value; } }
    [SerializeField] private ActionState actionState;

    //public InitiativeTracker initTracker;
    [SerializeField] private GridHighLighter gridHighLighter;
    [SerializeField] private CombatUI combatUI;
    public CombatUI CombatUI { get { return combatUI; } }

    [SerializeField] private AI.AICore ai_core;


    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.SLEEPING;
        combatantState = CombatantState.START;
        actionState = ActionState.NOT_SELECTED;
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

    public void RecieveMouseRightClick(Vector3 mousePos)
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
                        RecieveAction(mousePos);
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

        if (enemyCombatants != null)
        {
            combatants.AddRange(enemyCombatants);
        }
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

    // Utility Functions ***************************************************************************************************************************
    
    // use this function instead of manually altering the variable
    // so that other system variables can be edited along side state changes
    void SetCombatantState(CombatantState state)
    {
        combatantState = state;

        switch (state)
        {
            case CombatantState.MOVE:
                gridHighLighter.SetColour(SelectableTileColor.MOVEMENT);
                break;

            case CombatantState.ACTION:
                actionState = ActionState.NOT_SELECTED;
                gridHighLighter.SetColour(SelectableTileColor.ABILITY);
                break;

            case CombatantState.END:
                animEffectComplete = false;
                combatUI.deactivateAbilityButtons();
                combatUI.deactivateItemButtons();
                combatUI.deactivateChoiceButtons();
                break;
        }

    }

    // Start Turn Phase ****************************************************************************************************************************
    void StartTurn()
    {
        CycleQueue();

        currentCombatant.GetComponent<PathFindingUnit>().SetSelectableTiles(currentCombatant.GetComponent<Stats>().getStat(Combatant_Stats.Speed));

        SetCombatantState(CombatantState.MOVE);

        //Debug.Log("Start Turn");

        if (currentCombatant.tag == "Ally")
        {
            //Debug.Log("Player Move Phase");
        }
        else if (currentCombatant.tag == "Enemy")
        {
            //Debug.Log("Enemy Move Phase");
            AIMove();
        }
    }

    // Move Phase **********************************************************************************************************************************
    void TurnMovePhase()
    {
        if (currentCombatant.gameObject.tag == "Enemy")
        {
            AI.AIBaseBehavior behaviour = currentCombatant.GetComponent<AI.AIBaseBehavior>();
            if (behaviour)
            {
                if (behaviour.turn_completed)
                {
                    SetCombatantState(CombatantState.ACTION);
                }
            }
        }
        else
        {
            if(currentCombatant.GetComponent<PathFindingUnit>().PathFinished == true && currentlyMoving == true)
            {
                currentlyMoving = false;
                currentCombatant.GetComponent<PathFindingUnit>().PathFinished = false;
                SetCombatantState(CombatantState.ACTION);
                combatUI.activateChoiceButtons(uiPos);
            }

        }
    }

    void AIMove()
    {
        //SetCombatantState(CombatantState.ACTION);
        AI.AIBaseBehavior behaviour = currentCombatant.GetComponent<AI.AIBaseBehavior>();
        if (behaviour)
        {
            int speed = currentCombatant.GetComponent<Stats>().getStat(Combatant_Stats.Speed);

            behaviour.Move(ai_core, speed);
        }
    }

    public void RecieveMove(Vector3 mousePos)
    {
        if (currentCombatant.tag == "Ally" && gridHighLighter.IsTileSelectable(gridHighLighter.grid.WorldToNode(mousePos)))
        {
            uiPos = combatUI.WorldToCanvasSpace(mousePos);

            targetPos = mousePos;
            combatUI.activateMoveButtons(uiPos);
        }
    }

    public void OnConfirmMove()
    {
        combatUI.deactivateMoveButtons();
        recievedMoveCommand = false;
        currentCombatant.GetComponent<Combatant>().oldPosition = currentCombatant.GetComponent<Transform>().position;
        currentCombatant.GetComponent<PathFindingUnit>().RequestPath(new Vector2(targetPos.x, targetPos.y));

        gridHighLighter.ClearSelectableTiles();
        currentlyMoving = true;
        //SetCombatantState(CombatantState.ACTION);
        //combatUI.activateChoiceButtons(uiPos);
    }

    public void OnCancelMove()
    {
        combatUI.deactivateMoveButtons();
        recievedMoveCommand = false;
    }

    // Action Phase ********************************************************************************************************************************
    void TurnActionPhase()
    {
        // same as move phase function, unsure if this is needed
        
        if (currentCombatant.tag == "Enemy" && actionState == ActionState.NOT_SELECTED)
            AIAction();

        if (animEffectComplete)
        {
            animEffectComplete = false;
            //TODO: animate target, show health bar, show damage numbers, screen shake ect.
            //if (actionState == ActionState.ABILITY)
             //   DamagePopup.Create(abilityTargetPos, 300);

            StartCoroutine(Wait(1));
        }
            

        if (actionState == ActionState.FINISHED)
            SetCombatantState(CombatantState.END);
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        actionState = ActionState.FINISHED;
    }

    void AIAction()
    {
        AI.AIBaseBehavior behaviour = currentCombatant.GetComponent<AI.AIBaseBehavior>();
        if (behaviour)
        {
            actionState = ActionState.ABILITY;
            behaviour.Attack(ai_core);
        }

        actionState = ActionState.FINISHED;
    }

    public void RecieveAction(Vector3 mousePos)
    {
        switch (actionState)
        {
            case ActionState.ABILITY:
            {
                IsoNode node = gridHighLighter.grid.WorldToNode(mousePos);
                if (gridHighLighter.IsTileOccupied(node))
                {
                    // TODO edit this to allow for healing to target allies ect.
                    if (node.occupier.tag == "Enemy")
                    {
                        UseAbility(node.occupier, selectedAbility);
                        break;
                    }
                }
                break;
            }

            /*case ActionState.ITEM:
            {
                currentCombatant.GetComponent<Combatant>().UseItem()
                break;
            }//*/

        }

    }

    void UseAbility(GameObject target, int abilityIndex)
    {
        currentCombatant.GetComponent<TestCombatSystem>().enemy = target;
        AbilityResult result = currentCombatant.GetComponent<Combatant>().attackAbility(abilityIndex);

        abilityTargetPos = target.transform.position;

        AnimateAbility(target.transform.position,abilityIndex);
        StartCoroutine(ShowDamagePopup(0.2f, (int)result.oDamage, result.crit));
    }
    IEnumerator ShowDamagePopup(float seconds, int dmgNum, bool crit)
    {
        yield return new WaitForSeconds(seconds);
        DamagePopup.Create(abilityTargetPos, dmgNum, crit);
    }

    public void AnimateAbility(Vector3 animPos, int abilityIndex)
    {
        // draw animation effect 
        GameObject ability = Instantiate(currentCombatant.GetComponent<Combatant>().abilitiesUsing[abilityIndex].gameObject, animPos, Quaternion.identity);
        ability.GetComponent<Ability>().bManager = this;
        ability.transform.localScale = new Vector3(2f, 2f, 2f);
        ability.GetComponent<SpriteRenderer>().sortingOrder = 1;
        ability.GetComponent<Ability>().PlayAnim();
    }

    public void OnChooseAbility()
    {
        combatUI.activateAbilityButtons();
        combatUI.deactivateChoiceButtons();
        actionState = ActionState.ABILITY;
        if (currentCombatant.GetComponent<Combatant>().abilitiesLearnt[selectedAbility] != null)
            currentCombatant.GetComponent<PathFindingUnit>().SetSelectableTiles(currentCombatant.GetComponent<Combatant>().abilitiesLearnt[selectedAbility].abilityRange, true);
    }

    public void OnSelectAbility(int abilityIndex)
    {
        selectedAbility = abilityIndex;
        if (currentCombatant.GetComponent<Combatant>().abilitiesLearnt[selectedAbility] != null)
            currentCombatant.GetComponent<PathFindingUnit>().SetSelectableTiles(currentCombatant.GetComponent<Combatant>().abilitiesLearnt[selectedAbility].abilityRange, true);
    }

    public void OnChooseItem()
    {
        combatUI.activateItemButtons(uiPos);
        combatUI.deactivateChoiceButtons();
        actionState = ActionState.ITEM;
    }

    public void OnSelectItem(int itemIndex)
    {
        selectedItem = itemIndex;
        if(currentCombatant.GetComponent<Combatant>().combatantItems[itemIndex] != null)
        {
            currentCombatant.GetComponent<Combatant>().UseItem(itemIndex);

            // TODO: play animation or whatever

            //SetCombatantState(CombatantState.END);
            actionState = ActionState.FINISHED;
        }
            
    }

    public void OnBackButton()
    {
        combatUI.useBackButton(uiPos);
        gridHighLighter.ClearSelectableTiles();
        actionState = ActionState.NOT_SELECTED;
    }

    public void OnWait()
    {
        actionState = ActionState.FINISHED;
        //SetCombatantState(CombatantState.END);
    }


    // End Phase  **********************************************************************************************************************************
    void EndTurn()
    {
        // check if player has won or lost

        currentCombatant.GetComponent<PathFindingUnit>().OccupyTile(currentCombatant);

        SetCombatantState(CombatantState.START);
        //Debug.Log("End Turn");
    }

    public Queue<GameObject> getBattleQueue()
    {
        return battleQueue;
    }

    public CombatantState getCombatantState()
    {
        return combatantState;
    }

}