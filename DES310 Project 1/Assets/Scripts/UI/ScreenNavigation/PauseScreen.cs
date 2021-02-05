using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    Canvas pauseMenu;
    //public MasterInput controls;
    private bool paused = false;

    private void Awake()
    {
        pauseMenu = GetComponent<Canvas>();
        //controls = new MasterInput();
        //controls.PlayerControls.Pause.performed += ctx => PauseGame();
    }

    //private void OnEnable()
    //{
    //    controls.Enable();
    //}

    //private void OnDisable()
    //{
    //    controls.Disable();
    //}

    // Start is called before the first frame update
    void Start()
    {        
        if (pauseMenu)
        {
            pauseMenu.enabled = paused;
        }
    }

    public void PauseGame()
    {
        Debug.Log("Paused Pressed");
        if (pauseMenu && paused)
        {
            Time.timeScale = 0f;
            pauseMenu.enabled = paused;
            paused = !paused;
            Debug.Log("Game paused");
        }
        else if (pauseMenu && !paused)
        {
            Time.timeScale = 1f;
            pauseMenu.enabled = paused;
            paused = !paused;
            Debug.Log("Game unpaused");
        }
    }
}
