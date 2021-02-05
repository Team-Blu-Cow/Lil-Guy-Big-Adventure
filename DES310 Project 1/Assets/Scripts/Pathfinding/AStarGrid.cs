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
        transform.position = Vector3.zero;
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
                Vector3 worldPoint = NodeToWorld(x, y, 0);

                Vector3Int currentCell = tileMap.WorldToCell(worldPoint);

                bool walkable = tileMap.HasTile(currentCell);

                //decide whether point is walkable or not
                grid[x, y] = new AStarNode(walkable, worldPoint, new Vector3Int(x,y,0));
                grid[x, y].test = false;
            }
        }
    }

    public List<AStarNode> GetNeighbors(AStarNode _node)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        for (int x = -1; x <= 1; x ++)
        {
            for (int y = -1; y <= 1; y ++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = _node.gridPosition.x + x;
                int checkY = _node.gridPosition.y + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }

    private Vector3 NodeToWorld(float x, float y, float z)
    {
        return new Vector3(
            (transform.position.x) + (x-y) * (nodeDiameter),
            (transform.position.y) - (x+y) * (nodeRadius) - (nodeRadius),
            z
            );
    }

    public AStarNode WorldToNode(Vector3 _worldPos)
    {
        float node_x, node_y;
        int int_x, int_y;

        node_x = (((-_worldPos.y - nodeRadius) / nodeRadius) + (_worldPos.x / nodeDiameter)) / 2;
        node_y = (-_worldPos.y / nodeRadius) - node_x;

        int_x = Mathf.Clamp(Mathf.RoundToInt(node_x),0,gridSizeX-1);
        int_y = Mathf.Clamp(Mathf.RoundToInt(node_y) - 1,0,gridSizeY-1);

        return grid[int_x, int_y];
    }

    [SerializeField]
    public List<AStarNode> path;
    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;
            foreach (AStarNode node in grid)
            {
                //Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.color = (node.test) ? Color.blue : (node.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(node))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius/2));
            }
        }
    }
}


