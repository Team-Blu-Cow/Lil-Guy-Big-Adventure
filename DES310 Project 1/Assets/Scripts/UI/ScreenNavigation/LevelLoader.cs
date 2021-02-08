using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void SwitchScene(string in_Scene)
    {
        StartCoroutine(LoadLevel(in_Scene));
    }

    private IEnumerator LoadLevel(string in_Scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(in_Scene);
    }

    public void QuitAppliction()
    {
        Application.Quit();
        Debug.Log("Quit Called");
    }
}
