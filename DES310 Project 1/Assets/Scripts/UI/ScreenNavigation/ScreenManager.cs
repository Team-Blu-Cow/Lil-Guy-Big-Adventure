using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    #region Singleton
    public static ScreenManager screenManager;
    private void Awake()
    {
        if (screenManager != null)
        {
            Debug.LogWarning("Screen Manager instances");
            return;
        }
        screenManager = this;
        
        controls = new InputManager();
        controls.Keyboard.Pause.performed += ctx => TogglePause();
    }
    #endregion

    public InputManager controls;

    public PauseScreen pause;
    public SwapScreenCombatant combatantScreen;
    public SwapScreenParty partyScreen;
    public InventoryUI inventory;
    public HoverStats inGameParty;
    public LevelLoader levelSwitch;

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
    }
    
    public void OpenCombatantScreen(PartyCombatant combatant)
    {
        inGameParty.ToggleCanvas(false);
        partyScreen.CloseScreen();
        combatantScreen.OpenScreen(combatant);
        pause.TogglePauseGame(false);
    }
    
    public void OpenPartyScreen()
    {
        inGameParty.ToggleCanvas(false);
        partyScreen.OpenScreen();
        combatantScreen.CloseScreen();
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
    }

    public void SwitchLevel(string scene)
    {
        if (pause)
            pause.TogglePauseGame(false);
        levelSwitch.SwitchScene(scene);        
    }

}
