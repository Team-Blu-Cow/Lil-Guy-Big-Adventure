using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    public InputManager controls;

    [Header("Screens")]
    PauseScreen pause;
    SwapScreenCombatant combatantScreen;
    public SwapScreenCombatant CombatantScreen { get { return combatantScreen; } set { } }
    SwapScreenParty partyScreen;
    InventoryUI inventory;
    HoverStats inGameParty;
    BeastiaryScreen beastiaryScreen;
    InitiativeUI initiativeUI;
    RecruitEnemy recruitEnemy;
    RemoveParty removeParty;
    options option;

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
        recruitEnemy = GetComponentInChildren<RecruitEnemy>();
        removeParty = GetComponentInChildren<RemoveParty>();
        option = GetComponentInChildren<options>();
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

    //Helper
    /*public static List<Sprite> GetSpritesFromClip(AnimationClip clip)
    {
        var _sprites = new List<Sprite>();
        if (clip != null)
        {
            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                foreach (var frame in keyframes)
                {
                    _sprites.Add((Sprite)frame.value);
                }
            }
        }
        return _sprites;
    }*/

    public static Sprite GetFirstSprite(GameObject animGO)
    {
        /*if (animGO.TryGetComponent<Animator>(out Animator anim))
            return GetSpritesFromClip(anim.runtimeAnimatorController.animationClips[0])[0];*/
        if (animGO.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            return sr.sprite;
        else
            return null;
    }

    //-------------------------------------  UI -------------------------------------\\

    // Pause //////
    public void TogglePause()
    {
        if (inGame)
        {
            pause.TogglePauseGame(true);
        }
        else
        {
            if (option.GetComponent<Canvas>().enabled)
            {
                option.GetComponent<Canvas>().enabled = false;
                if (AudioManager.instance)
                {
                    AudioManager.instance.FadeOut("One Shot Test");
                    AudioManager.instance.FadeOut("Ambient Test");
                }
                pause.TogglePauseGame(true);
            }
            else
            {
                pause.TogglePauseGame(false);
                OpenInGameUI();
            }
        }
        
    }
    
    // Combatant ///////
    public void OpenCombatantScreen(PartyCombatant combatant)
    {
        inGame = false;
        inGameParty.ToggleCanvas(false);
        partyScreen.CloseScreen();
        combatantScreen.OpenScreen(combatant);
        combatantScreen.ToggleLearned(false);
    }
    
    // Party ////////
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

    // Beastiary screen //////////////
    public void OpenBeastiaryScreen()
    {
        inGame = false;
        beastiaryScreen.OpenScreen();
        inGameParty.ToggleCanvas(false);
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
    }
    
    public void ToggleBeastiaryScreen()
    {
        if (beastiaryScreen.GetComponent<Canvas>().enabled)
        {
            inGame = true;
            beastiaryScreen.CloseScreen();
        }
        else
        {
            inGame = false;
            beastiaryScreen.OpenScreen();
        }
    }
    
    // Inventory /////////////
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
    
    // In Game Ui //////////////////// 
    public void OpenInGameUI()
    {
        inGame = true;
        inGameParty.ToggleCanvas(true);
        partyScreen.CloseScreen();
        combatantScreen.CloseScreen();
        combatantScreen.ToggleLearned(false);
        beastiaryScreen.CloseScreen();
    }

    // Helper Functions /////////////////
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

    // Initiative ///////////
    public void CloseInititive()
    {
        initiativeUI.GetComponentInParent<Canvas>().enabled = false;
    }
    
    public void OpenInititive()
    {
        initiativeUI.GetComponentInParent<Canvas>().enabled = true;
    }

    // Recruit /////////////////
    public void ShowRecruit()
    {
        if(recruitEnemy != null && recruitEnemy.TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.enabled = true;
            recruitEnemy.Recruit();
        }
    }
    
    public void HideRecruit()
    {
        recruitEnemy.GetComponent<Canvas>().enabled = false;
    }
    
    public void ShowRemove(GameObject newPartyMember)
    {
        removeParty.GetComponent<Canvas>().enabled = true;
        removeParty.Remove(newPartyMember);
    }
    
    public void HideRemove()
    {
        removeParty.GetComponent<Canvas>().enabled = false;
    }

    // Options //////////////
    public void ShowOptions()
    {
        inGame = false;
        pause.GetComponent<Canvas>().enabled = false;
        option.GetComponent<Canvas>().enabled = true;
        option.GetComponent<GraphicRaycaster>().enabled = true;
    }
}
