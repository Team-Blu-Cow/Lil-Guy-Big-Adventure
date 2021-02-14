using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[10];
    [SerializeField] public Vector3[] path;
    [SerializeField] private GameObject pathNodePrefab;
    private List<GameObject> nodes;

    public void GetPath(Vector3 startPos, Vector3 endPos)
    {
        PathRequestManager.RequestPath(transform.position, endPos, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;

            // create path objects
        }
    }
}
