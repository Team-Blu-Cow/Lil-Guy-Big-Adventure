using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    public InputManager controls;

    [Header("Screens")]
    PauseScreen pause;
    SwapScreenCombatant combatantScreen;
    SwapScreenParty partyScreen;
    InventoryUI inventory;
    HoverStats inGameParty;
    BeastiaryScreen beastiaryScreen;
    InitiativeUI initiativeUI;

    LevelLoader levelSwitch;

    [SerializeField] List<TMP_FontAsset> fonts;
    [HideInInspector] public TMP_FontAsset activeFont;
    public PlayerPartyManager partyManager;
    bool inGame = true;

    public HoverStats hoverStats
    {
        get
        {
            return instance.inGameParty;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Screen Manager instances");
            return;
        }
        instance = this;
        
        controls = new InputManager();
        controls.Keyboard.Pause.performed += ctx => TogglePause();

        pause = GetComponentInChildren<PauseScreen>();
        combatantScreen = GetComponentInChildren<SwapScreenCombatant>();
        beastiaryScreen = GetComponentInChildren<BeastiaryScreen>();
        partyScreen = GetComponentInChildren<SwapScreenParty>();
        inventory = GetComponentInChildren<InventoryUI>();
        inGameParty = GetComponentInChildren<HoverStats>();
        initiativeUI = GetComponentInChildren<InitiativeUI>();
        levelSwitch = GetComponentInChildren<LevelLoader>();

        activeFont = fonts[0];
        SwapFont(0);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void TogglePause()
    {
        if (inGame)
        {
            if (Time.timeScale == 0f)
            {
                pause.TogglePauseGame(false);
            }
            else
            {
                pause.TogglePauseGame(true);
            }
        }
        else
        {
            OpenInGameUI();
        }
        
    }
    
    public void OpenCombatantScreen(PartyCombatant combatant)
    {
        inGame = false;
        inGameParty.ToggleCanvas(false);
        partyScreen.CloseScreen();
        combatantScreen.OpenScreen(combatant);
        combatantScreen.ToggleLearned(false);
    }
    
    public void OpenPartyScreen()
    {
        inGame = false;
        inGameParty.ToggleCanvas(false);
        partyScreen.OpenScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
        beastiaryScreen.CloseScreen();
        pause.TogglePauseGame(false);
    }

    public void OpenBeastiaryScreen()
    {
        inGame = false;
        beastiaryScreen.OpenScreen();
        inGameParty.ToggleCanvas(false);
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
    }
    
    public void OpenInventory()
    {
        inventory.ToggleInventory(true);
    }
    public void CloseInventory()
    {
        inventory.ToggleInventory(false);
    }
    
    public void ToggleInventory()
    {
        inventory.FlipInventory();
    }
    
    public void OpenInGameUI()
    {
        inGame = true;
        inGameParty.ToggleCanvas(true);
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
        beastiaryScreen.CloseScreen();
    }

    public void SwitchLevel(string scene)
    {
        if (pause)
            pause.TogglePauseGame(false);
        levelSwitch.SwitchScene(scene);        
    }

    public void SwapFont(int i)
    {
        activeFont = fonts[i];
        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (fonts.Count > i)
                text.font = fonts[i];
        }
    }

    public SwapScreenCombatant GetCombatantScreen()
    {
        return combatantScreen;
    }

    public void CloseInititive()
    {
        initiativeUI.GetComponentInParent<Canvas>().enabled = false;
    }
    
    public void OpenInititive()
    {
        initiativeUI.GetComponentInParent<Canvas>().enabled = true;
    }
}
