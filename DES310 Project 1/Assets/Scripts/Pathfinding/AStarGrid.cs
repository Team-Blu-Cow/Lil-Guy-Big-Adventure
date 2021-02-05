using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    public Vector2Int gridSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;

    public Tilemap tileMap;

    AStarNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x);
        gridSizeY = Mathf.RoundToInt(gridSize.y);
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

                Vector3Int currentCell = tileMap.WorldToCell(worldPoint);

                bool walkable = tileMap.HasTile(currentCell);

                //decide whether point is walkable or not
                grid[x, y] = new AStarNode(walkable, worldPoint, new Vector3Int(x,y,1));
            }
        }
    }

    private Vector3 ToWorld(float x, float y, Vector3 origin)
    {
        return new Vector3(
            (transform.position.x * nodeDiameter) + (x-y) * (nodeRadius*2),
            (transform.position.y * nodeDiameter) - (x+y) * (nodeRadius) - (nodeRadius),
            0
            );
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));
       

        if (grid != null)
        {
            Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;
            foreach (AStarNode node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius/2));
            }
        }
    }

    private float NormalizeInRange(float p, float p_min, float p_max)
    {
        return (p - p_min) / (p_max - p_min);
    }
}


