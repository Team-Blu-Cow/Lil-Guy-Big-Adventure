using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class IsoNode : IHeapItem<IsoNode>
{
    // Universal Path Finding Members *************************************************************
    public bool walkable;
    public bool occupied = false;

    public GameObject occupier;

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
    public bool selectable = false;

    public int distance = 0;

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

    public bool IsTraversable()
    {
        if (!walkable)
            return false;
        else if (occupied/*&& any other parameters */)
        {
            return false;
        }
        return true;
    }

    public void SetOccupied(GameObject _occupier)
    {
        if(_occupier == null)
        {
            occupied = false;
            occupier = null;
        }
        else
        {
            occupied = true;
            occupier = _occupier;
        }
    }
}
