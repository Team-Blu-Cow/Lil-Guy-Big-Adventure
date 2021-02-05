using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public bool walkable;
    public Vector3 worldPosition;
    public Vector3Int gridPosition;

    public AStarNode(bool _walkable, Vector3 _worldPos, Vector3Int _gridPosition)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridPosition = _gridPosition;
    }
}
