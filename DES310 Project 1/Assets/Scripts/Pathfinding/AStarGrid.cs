using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public Vector2 gridSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    AStarNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];
        Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = ToWorld(x, y, worldTopLeft);

                //Vector3 worldPoint = worldTopLeft + Vector3.right * (x * nodeDiameter + nodeRadius) - Vector3.up * (y * nodeDiameter + nodeRadius);
                //decide whether point is walkable or not
                grid[x, y] = new AStarNode(true, worldPoint);
            }
        }
    }

    private Vector3 ToWorld(float x, float y, Vector3 origin)
    {
        return new Vector3(
            (transform.position.x * nodeDiameter) + (x-y) * (nodeRadius*2),
            (origin.y * nodeDiameter) - (x+y) * (nodeRadius),
            1
            );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));
       

        if (grid != null)
        {
            Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;
            foreach (AStarNode node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;


                /*Gizmos.color = new Color(
                    NormalizeInRange(node.worldPosition.x,worldTopLeft.x, worldTopLeft.x + gridSize.x), 
                    NormalizeInRange(node.worldPosition.y, worldTopLeft.y, worldTopLeft.y - gridSize.y),
                    0, 
                    1);//*/
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter/2));
            }
        }
    }

    private float NormalizeInRange(float p, float p_min, float p_max)
    {
        return (p - p_min) / (p_max - p_min);
    }
}


