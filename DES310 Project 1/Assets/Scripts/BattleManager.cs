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
    [SerializeField] private GameObject playerPartyGO;

    [SerializeField] private List<GameObject> combatants;
    [SerializeField] private Queue<GameObject> battleQueue;
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
    public BattleState BattleState { get { return battleState; } set { battleState = value; } }
    [SerializeField] private CombatantState combatantState;
    public CombatantState CombatantState { get { return combatantState; } set { combatantState = value; } }
    [SerializeField] private ActionState actionState;

    //public InitiativeTracker initTracker;
    [SerializeField] private GridHighLighter gridHighLighter;

    [SerializeField] private CombatUI combatUI;
    public CombatUI CombatUI { get { return combatUI; } }

    [SerializeField] private AI.AICore ai_core;

    private void Awake()
    {        
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerParty = FindObjectOfType<PlayerPartyManager>();
        if (playerParty == null)
        {
            playerParty = Instantiate(playerPartyGO).gameObject.GetComponent<PlayerPartyManager>();
          
        }
        playerParty.GetComponentsInChildren<PathFindingUnit>()[0].GridHighLighter = gridHighLighter;
        playerParty.GetComponentsInChildren<Movement>()[0].grid = gridHighLighter.grid;
        ScreenManager.instance.partyManager = playerParty;
        playerParty.GetComponentsInChildren<PathFindingUnit>()[0].target = FindObjectOfType<CursorController>().transform;


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
    private void SetBattleQueue()
    {
        battleQueue = new Queue<GameObject>();

        foreach (var combatant in combatants)
        {
            battleQueue.Enqueue(combatant);
        }
    }

    private void CycleQueue()
    {
        currentCombatant = battleQueue.Dequeue();
        while (currentCombatant.activeSelf == false)
        {
            currentCombatant = battleQueue.Dequeue();
        }
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
                if (combatants[j - 1].GetComponent<Stats>().GetStat(Combatant_Stats.Initiative) < combatants[j].GetComponent<Stats>().GetStat(Combatant_Stats.Initiative))
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

    public void CheckIfCombatantDead(GameObject combatant)
    {
        if (combatant != null && combatants.Contains(combatant) && combatant.GetComponent<Combatant>().GetComponent<Stats>().GetStat(Combatant_Stats.HP) <= 0)
        {
            if (combatant.GetComponent<Combatant>().GetComponent<Stats>().combatant_type == Combatant_Type.Human)
            {
                // KILL MAIN CHARACTER
                Debug.Log("Player died, you suck!");
            }
            else
            {
                if (combatant.tag == "Enemy")
                {
                    enemyCombatants.Remove(combatant);
                }
                else
                {
                    //TODO: remove combatant from player's party
                    ai_core.party_list.Remove(combatant);
                }

                combatants.Remove(combatant);
                combatant.GetComponent<PathFindingUnit>().OccupyTile(null);
                combatant.SetActive(false);
            }
        }
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
                        if (actionState != ActionState.ITEM)
                        {
                            RecieveAction(mousePos);
                        }
                        break;
                }
                break;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    // BATTLE STATE METHODS                                                                                                                       //
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    private void StartBattle()
    {
        combatants = new List<GameObject>();

        ai_core.party_list = new List<GameObject>();

        if (enemyCombatants != null)
        {
            combatants.AddRange(enemyCombatants);
        }
        foreach (var member in playerParty.party)
        {
            if (member != null)
            {
                combatants.Add(member);
                ai_core.party_list.Add(member);
            }
        }

        if (playerParty.party[0].TryGetComponent<Movement>(out Movement move))
        {
            move.enabled = false;
        }

        SortBattleInitiative();
        SetBattleQueue();

        //currentCombatantIndex = 0;

        battleState = BattleState.IN_BATTLE;
    }

    private void InBattle()
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
    private void SetCombatantState(CombatantState state)
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

    public void AddParty(GameObject combatant)
    {
        playerParty.AddCombatant(combatant);
    }

    // Start Turn Phase ****************************************************************************************************************************
    private void StartTurn()
    {
        receivedActionCommand = false;

        CycleQueue();

        currentCombatant.GetComponent<PathFindingUnit>().SetSelectableTiles(currentCombatant.GetComponent<Stats>().GetStat(Combatant_Stats.Speed));

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
    private void TurnMovePhase()
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
            if (currentCombatant.GetComponent<PathFindingUnit>().PathFinished == true && currentlyMoving == true)
            {
                currentlyMoving = false;
                currentCombatant.GetComponent<PathFindingUnit>().PathFinished = false;
                SetCombatantState(CombatantState.ACTION);
                combatUI.activateChoiceButtons(uiPos);
            }
        }
    }

    private void AIMove()
    {
        //SetCombatantState(CombatantState.ACTION);
        AI.AIBaseBehavior behaviour = currentCombatant.GetComponent<AI.AIBaseBehavior>();
        if (behaviour)
        {
            int speed = currentCombatant.GetComponent<Stats>().GetStat(Combatant_Stats.Speed);

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
    private void TurnActionPhase()
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

            StartCoroutine(WaitThenFinish(1));
        }

        if (actionState == ActionState.FINISHED)
            SetCombatantState(CombatantState.END);
    }

    private IEnumerator WaitThenFinish(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        actionState = ActionState.FINISHED;
    }

    private void AIAction()
    {
        AI.AIBaseBehavior behaviour = currentCombatant.GetComponent<AI.AIBaseBehavior>();
        if (behaviour)
        {
            actionState = ActionState.ABILITY;
            AbilityResult result = behaviour.Attack(ai_core);
            if (result.target != null)
            {
                abilityTargetPos = result.target.transform.position;

                CheckIfCombatantDead(result.target);

                AnimateAbility(abilityTargetPos, result.abilityIndex);
                StartCoroutine(ShowDamagePopup(0.2f, (int)result.oDamage, result.crit));
            }
            else
            {
                actionState = ActionState.FINISHED;
            }
        }
        else
        {
            actionState = ActionState.FINISHED;
        }
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
                        if (receivedActionCommand == false)
                        {
                            //Check ability type                 
                            switch (currentCombatant.GetComponent<Combatant>().abilitiesUsing[selectedAbility].abilityType)
                            {
                                case ability_type.Damage:
                                    if (node.occupier.tag == ("Enemy"))
                                    {
                                        UseAbility(node.occupier, selectedAbility);
                                        receivedActionCommand = true;
                                    }
                                    break;

                                case ability_type.Heal:
                                    if (node.occupier.tag == ("Ally"))
                                    {
                                        UseAbility(node.occupier, selectedAbility);
                                        receivedActionCommand = true;
                                    }
                                    break;

                                case ability_type.Buff:
                                    if (node.occupier.tag == ("Ally"))
                                    {
                                        UseAbility(node.occupier, selectedAbility);
                                        receivedActionCommand = true;
                                    }
                                    break;
                            }                                                
                        }
                    }
                    break;
                }

                case ActionState.ITEM:
                {
                    currentCombatant.GetComponent<Combatant>().UseItem(selectedItem);
                    break;
                }
        }
    }

    private void UseAbility(GameObject target, int abilityIndex)
    {
        currentCombatant.GetComponent<CombatSystem>().target = target;
        AbilityResult result = currentCombatant.GetComponent<Combatant>().UseAbility(abilityIndex);
        abilityTargetPos = target.transform.position;

        CheckIfCombatantDead(target);

        AnimateAbility(target.transform.position, abilityIndex);
        StartCoroutine(ShowDamagePopup(0.2f, (int)result.oDamage, result.crit));
    }

    private IEnumerator ShowDamagePopup(float seconds, int dmgNum, bool crit)
    {
        yield return new WaitForSeconds(seconds);
        DamagePopup.Create(abilityTargetPos, dmgNum, crit);
    }

    public void AnimateAbility(Vector3 animPos, int abilityIndex)
    {
        // draw animation effect
        /*GameObject ability = Instantiate(currentCombatant.GetComponent<Combatant>().abilitiesUsing[abilityIndex].gameObject, animPos, Quaternion.identity);
        ability.GetComponent<Ability>().bManager = this;
        ability.transform.localScale = new Vector3(2f, 2f, 2f);
        ability.GetComponent<SpriteRenderer>().sortingOrder = 1;
        ability.GetComponent<Ability>().PlayAnim();*/
        if (currentCombatant.GetComponent<Combatant>().abilitiesUsing[abilityIndex].Anim != null)
        {
            animPos = new Vector3(animPos.x, animPos.y - 0.125f, 3);
            GameObject ability = Instantiate(currentCombatant.GetComponent<Combatant>().abilitiesUsing[abilityIndex].Anim, animPos, Quaternion.identity);
            ability.transform.localScale = Vector3.one * currentCombatant.GetComponent<Combatant>().abilitiesUsing[abilityIndex].scale;
            StartCoroutine(WaitForAnim(currentCombatant.GetComponent<Combatant>().abilitiesUsing[abilityIndex].time));
        }
    }

    private IEnumerator WaitForAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animEffectComplete = true;
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
        combatUI.colorAbilityButton(abilityIndex);
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
        if (currentCombatant.GetComponent<Combatant>().combatantItems[itemIndex] != null)
        {
            // TODO: play animation or whatever

            RecieveAction(new Vector3());

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
    private void EndTurn()
    {
        // check if player has won or lost
        if (enemyCombatants.Count <= 0)
        {
            EndBattle();
        }

        currentCombatant.GetComponent<PathFindingUnit>().OccupyTile(currentCombatant);

        SetCombatantState(CombatantState.START);

        
        //Debug.Log("End Turn");
    }
    void EndBattle()
    {
        battleState = BattleState.FINISHED;

        foreach (GameObject exit in FindObjectOfType<MapGeneration>().placedExits)
        {
            exit.SetActive(true);
        }

        foreach (GameObject combatant in playerParty.party)
        {
            if (combatant)
            {
                combatant.GetComponent<Stats>().ResetStats();
            }
        }

        if (playerParty.party[0].TryGetComponent<Movement>(out Movement move))
        {
            move.enabled = true;
        }

        gridHighLighter.ClearSelectableTiles();
        Debug.Log("battle won");
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