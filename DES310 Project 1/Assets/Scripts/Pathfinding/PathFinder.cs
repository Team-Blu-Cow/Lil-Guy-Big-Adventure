using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public AStarGrid grid;

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {

    }

    void Update()
    {
        AStarNode node = grid.ToNode(transform.position);
        node.test = true;
    }

}
