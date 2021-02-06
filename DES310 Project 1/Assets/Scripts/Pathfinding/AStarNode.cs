using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class AStarNode : IHeapItem<AStarNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public Vector3Int gridPosition;

    public AStarNode parent;
    int m_heapIndex = 0;

    public int gCost;
    public int hCost;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int heapIndex
    {
        get
        {
            return m_heapIndex;
        }
        set
        {
            m_heapIndex = value;
        }
    }

    public AStarNode(bool _walkable, Vector3 _worldPos, Vector3Int _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridPosition = _gridPosition;
    }
    
    public int CompareTo(AStarNode comparisonNode)
    {
        int compare = fCost.CompareTo(comparisonNode.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(comparisonNode.hCost);
        }

        return -compare;
    }
}
