using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private Animator transition;
    [SerializeField] [Range(1, 100)] private float transitionTime = 1f;

    private void Start()
    {
        transition = GetComponentInChildren<Animator>();
    }

    public void SwitchScene(string in_Scene)
    {
        StartCoroutine(LoadLevel(in_Scene));
    }

    private IEnumerator LoadLevel(string in_Scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(in_Scene);
    }

    public void QuitAppliction()
    {
        Application.Quit();
        Debug.Log("Quit Called");
    }
}