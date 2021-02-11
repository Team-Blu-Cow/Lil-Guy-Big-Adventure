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

        if (startNode.walkable && targetNode.walkable)
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
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
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
        Vector2 directionOld = Vector2.zero;

        for(int i = 0; i < path.Count-1; i++)
        {
            //Vector2 directionNew = new Vector2(path[i].gridPosition.x - path[i+1].gridPosition.x, path[i].gridPosition.y - path[i+1].gridPosition.y);
            //if (directionNew != directionOld)
            //{
                waypoints.Add(new Vector3(path[i].worldPosition.x, path[i].worldPosition.y, 2));
            //}
            //directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    int GetDistance(IsoNode nodeA, IsoNode nodeB)
    {
        // TODO: change this, this is just for the tutorial
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        return (dstX*10) + (dstY*10);

        /*if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        else
            return 14 * dstX + 10 * (dstY - dstX);//*/
    }

}
