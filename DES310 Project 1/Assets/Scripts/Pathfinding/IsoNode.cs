using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class IsoNode : IHeapItem<IsoNode>
{
    // Universal Path Finding Members *************************************************************
    public bool walkable;
    public IsoNode parent;

    // A* Members *********************************************************************************
    int m_heapIndex = 0;

    public int gCost;
    public int hCost;
    public int fCost
    {
        get{return gCost + hCost;}
    }
    public int heapIndex
    {
        get { return m_heapIndex; }
        set { m_heapIndex = value; }
    }

    // BFS Members ********************************************************************************
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    // list of adjacent tiles

    public bool visited;
    public int distance;

    // Position & Index Members *******************************************************************
    public Vector3 worldPosition;
    public Vector3Int gridPosition;

    public IsoNode(bool _walkable, Vector3 _worldPos, Vector3Int _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridPosition = _gridPosition;
    }
    
    public int CompareTo(IsoNode comparisonNode)
    {
        int compare = fCost.CompareTo(comparisonNode.fCost);
        if (compare == 0)
            compare = hCost.CompareTo(comparisonNode.hCost);

        return -compare;
    }
}
