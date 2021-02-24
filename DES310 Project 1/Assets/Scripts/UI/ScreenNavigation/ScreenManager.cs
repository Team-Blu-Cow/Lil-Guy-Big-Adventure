using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    public InputManager controls;

    PauseScreen pause;
    SwapScreenCombatant combatantScreen;
    SwapScreenParty partyScreen;
    InventoryUI inventory;
    HoverStats inGameParty;
    BeastiaryScreen beastiaryScreen;
    LevelLoader levelSwitch;
    public List<TMP_FontAsset> fonts;
    public PlayerPartyManager partyManager;

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
        levelSwitch = GetComponentInChildren<LevelLoader>();
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
        pause.TogglePauseGame(true);
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        beastiaryScreen.CloseScreen();
    }
    
    public void OpenCombatantScreen(PartyCombatant combatant)
    {
        inGameParty.ToggleCanvas(false);
        partyScreen.CloseScreen();
        combatantScreen.OpenScreen(combatant);
        combatantScreen.ToggleLearned(false);
        pause.TogglePauseGame(false);
    }
    
    public void OpenPartyScreen()
    {
        inGameParty.ToggleCanvas(false);
        partyScreen.OpenScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
        pause.TogglePauseGame(false);
    }
    
    public void ToggleBeastiaryScreen()
    {
        inGameParty.ToggleCanvas(!beastiaryScreen.ToggleScreen());
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
        pause.TogglePauseGame(false);
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
        inGameParty.ToggleCanvas(true);
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        pause.TogglePauseGame(false);
        combatantScreen.ToggleLearned(false);
    }

    public void SwitchLevel(string scene)
    {
        if (pause)
            pause.TogglePauseGame(false);
        levelSwitch.SwitchScene(scene);        
    }

    public void SwapFont(int i)
    {
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
}
