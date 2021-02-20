using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashScreen : MonoBehaviour
{
    private LevelLoader levelLoader;
    private AudioManager audioManager;

    // Start is called before the first frame update
    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        levelLoader = FindObjectOfType<LevelLoader>();
        StartCoroutine(SplashScreenOrder());
    }

    private IEnumerator SplashScreenOrder()
    {
        yield return new WaitForSeconds(1f);
        audioManager.Play("Main Theme");
        yield return new WaitForSeconds(2f);
        levelLoader.SwitchScene("MainMenu");
    }

    // Update is called once per frame
    private void Update()
    {
    }
}