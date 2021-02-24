using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOutliner : MonoBehaviour
{
    GameObject parent;

    private void Awake()
    {
        parent = transform.parent.gameObject;
        GetComponent<SpriteRenderer>().sprite = parent.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sprite = parent.GetComponent<SpriteRenderer>().sprite;
    }
}
