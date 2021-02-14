using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathFinder : MonoBehaviour
{
    PathRequestManager requestManager;
    IsoGrid grid;

    private void Awake()
    {
        grid = gameObject.GetComponent<IsoGrid>();
        requestManager = gameObject.GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        IsoNode startNode = grid.WorldToNode(startPos);
        IsoNode targetNode = grid.WorldToNode(targetPos);

        if (startNode.IsTraversable() && targetNode.IsTraversable())
        {
            Heap<IsoNode> openSet = new Heap<IsoNode>(grid.MaxSize);
            HashSet<IsoNode> closedSet = new HashSet<IsoNode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {

                IsoNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    UnityEngine.Debug.Log("Path found in: " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (IsoNode neighbour in grid.GetNeighbors(currentNode))
                {
                    if (!neighbour.IsTraversable() || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }

                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);

    }

    Vector3[] RetracePath(IsoNode startNode, IsoNode endNode)
    {
        List<IsoNode> path = new List<IsoNode>();
        IsoNode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(currentNode);
        Vector3[] waypoints = SimplifyPath(path);

        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<IsoNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        for(int i = 0; i < path.Count-1; i++)
        {
            waypoints.Add(new Vector3(path[i].worldPosition.x, path[i].worldPosition.y, 2));
        }

        return waypoints.ToArray();
    }

    int GetDistance(IsoNode nodeA, IsoNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        return dstX + dstY;
    }

}
