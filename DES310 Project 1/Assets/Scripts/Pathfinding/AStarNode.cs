using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AStarNode
{
    public bool walkable;
    public Vector3 worldPosition;

    public AStarNode(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
    }
}
