using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUnit : MonoBehaviour
{
    public Transform target;
    [SerializeField] float speed = 0.5f;
    [SerializeField] Vector3[] path;
    int targetIndex;
    bool currentlyPathFinding = false;
    [SerializeField] private GridHighLighter gridHighLighter;
    
    public void SetSelectableTiles(int range)
    {
        gridHighLighter.SetSelectableTiles(gridHighLighter.grid.WorldToNode(transform.position), range);
    }

    public void RequestPath()
    {
        if (!currentlyPathFinding)
        {
            if (gridHighLighter.IsTileSelectable(gridHighLighter.grid.WorldToNode(target.position)))
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            else
                Debug.Log("Tile is not a valid move position");
        }   
    }

    public void RequestPath(Vector3 targetPos)
    {
        if (!currentlyPathFinding)
        {
            if (gridHighLighter.IsTileSelectable(gridHighLighter.grid.WorldToNode(targetPos)))
                PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
            else
                Debug.Log("Tile is not a valid move position");
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            currentlyPathFinding = true;
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public void StartPath()
    {
        StopPath();
        RequestPath();
    }

    public void StopPath()
    {
        if (currentlyPathFinding)
        {
            targetIndex = 0;
            path = new Vector3[0];
            currentlyPathFinding = false;
            StopCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    currentlyPathFinding = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed*Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.125f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
