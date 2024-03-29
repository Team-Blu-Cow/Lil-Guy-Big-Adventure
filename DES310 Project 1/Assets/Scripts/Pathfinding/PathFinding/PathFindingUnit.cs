using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUnit : MonoBehaviour
{
    public Transform target;
    [SerializeField] float speed = 0.5f;
    [SerializeField] Vector3[] path;
    int targetIndex;
    [HideInInspector] public bool currentlyPathFinding = false;
    [HideInInspector] public bool PathFinished = false;
    [SerializeField] private GridHighLighter gridHighLighter;
    public GridHighLighter GridHighLighter { get {return gridHighLighter; } set {gridHighLighter = value; } }

    private void Start()
    {
        //IsoNode node = gridHighLighter.grid.WorldToNode(transform.position);
        OccupyTile(gameObject);
    }

    public void OccupyTile(GameObject me)
    {
        if(gridHighLighter != null && gridHighLighter.grid != null)
        {
            IsoNode node = gridHighLighter.grid.WorldToNode(transform.position);
            node.SetOccupied(me);
        }
    }

    public void SetSelectableTiles(int range, bool includeOccupied = false)
    {
        OccupyTile(null);
        gridHighLighter.SetSelectableTiles(gridHighLighter.grid.WorldToNode(transform.position), range, includeOccupied);
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

    public bool RequestPath(Vector3 targetPos)
    {
        if (!currentlyPathFinding)
        {
            if (gridHighLighter.IsTileSelectable(gridHighLighter.grid.WorldToNode(targetPos)))
            {
                PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                return true;
            }
            else
            {
                Debug.Log("Tile is not a valid move position");
                return false;
            }
        }

        return false;
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
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
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
        if(path != null)
        {
            if(path.Length > 0)
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
                            PathFinished = true;
                            yield break;
                        }
                        currentWaypoint = path[targetIndex];
                    }

                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    yield return null;
                }
            }
        }


        currentlyPathFinding = false;
        PathFinished = true;
        yield return null;
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
