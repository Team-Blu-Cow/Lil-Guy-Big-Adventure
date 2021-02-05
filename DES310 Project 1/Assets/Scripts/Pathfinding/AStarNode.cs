using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AStarNode
{
    public bool walkable;
    public Vector3 worldPosition;
    public Vector3Int gridPosition;

    public int gCost;
    public int hCost;

    public AStarNode parent;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public AStarNode(bool _walkable, Vector3 _worldPos, Vector3Int _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridPosition = _gridPosition;
    }
    

}
