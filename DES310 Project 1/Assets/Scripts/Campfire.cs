using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [HideInInspector] public InputManager input;
    private PlayerPartyManager playerParty;
    private Transform cursor;
    public Vector3 cursorPos;

    private GameObject textInstructionObject;

    void Awake()
    {
        input = new InputManager();

        textInstructionObject = gameObject.GetComponentInChildren<TMPro.TextMeshPro>().gameObject;
        textInstructionObject.SetActive(true);
        input.Keyboard.LClick.performed += ctx => MouseLeftClick();
    }
    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        cursor = FindObjectOfType<CursorController>().transform;
        playerParty = FindObjectOfType<PlayerPartyManager>();
        playerParty.party[0].transform.position = new Vector3(-3.0f, -2.25f, 2.0f);
    }



    // Update is called once per frame
    void Update()
    {
        cursorPos = cursor.position;
    }
    
    private void MouseLeftClick()
    {
        // Debug.Log(":]");
        if(cursorPos.x > -1.0f && cursorPos.x < 0.0f && cursorPos.y < -4.25 && cursorPos.y > -4.75)
        {
            for(int i = 0; i < playerParty.party.Length; i++)
            {
                if(playerParty.party[i] != null)
                {
                    playerParty.party[i].GetComponent<Stats>().SetModStat(Combatant_Stats.HP, playerParty.party[i].GetComponent<Stats>().GetStat(Combatant_Stats.Constitution));
                }
            }
            textInstructionObject.SetActive(false);
        }
    }

}
