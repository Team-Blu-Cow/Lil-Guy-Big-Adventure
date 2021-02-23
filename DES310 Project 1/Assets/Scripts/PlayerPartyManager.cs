using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPartyManager : MonoBehaviour
{
    public GameObject[] party = new GameObject[4];

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Remove()
    {
        foreach (GameObject item in GetComponentsInChildren<GameObject>())
        {
            Destroy(item);
        }
    }
}
