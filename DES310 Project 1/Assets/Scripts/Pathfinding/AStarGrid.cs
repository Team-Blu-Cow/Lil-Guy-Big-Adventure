using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] public Vector2Int gridSize;

    [Header("Node Settings")]
    public float nodeRadius;
    public LayerMask unwalkableMask;

    [Header("References")]
    public Tilemap tileMap;

    AStarNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        InitGrid();
        CreateGrid();
    }

    public void InitGrid()
    {
        transform.position = Vector3.zero;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x);
        gridSizeY = Mathf.RoundToInt(gridSize.y);
    }

    public void CreateGrid()
    {
        grid = new AStarNode[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = NodeToWorld(x, y, 0);

                Vector3Int currentCell = tileMap.WorldToCell(worldPoint);

                bool walkable = tileMap.HasTile(currentCell);

                grid[x, y] = new AStarNode(walkable, worldPoint, new Vector3Int(x,y,0));
            }
        }
    }

    public List<AStarNode> GetNeighbors(AStarNode _node)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        int checkX, checkY;

        // north neighbor
        checkX = _node.gridPosition.x;
        checkY = _node.gridPosition.y + 1;

        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        // south neighbor
        checkX = _node.gridPosition.x;
        checkY = _node.gridPosition.y - 1;

        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        // east neighbor
        checkX = _node.gridPosition.x + 1;
        checkY = _node.gridPosition.y;

        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        // west neighbor
        checkX = _node.gridPosition.x - 1;
        checkY = _node.gridPosition.y;

        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        {
            neighbors.Add(grid[checkX, checkY]);
        }

        /*for (int x = -1; x <= 1; x ++)
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
        }//*/

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
    
    private void SnapToGrid()
    {
        Vector3 position = new Vector3(
            Mathf.Round(this.transform.position.x / this.gridSize.x) * this.gridSize.x,
            Mathf.Round(this.transform.position.y / this.gridSize.y) * this.gridSize.y,
            this.transform.position.z
            );

        this.transform.position = position;
    }

    [HideInInspector]
    public List<AStarNode> path;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x + (gridSize.x*nodeDiameter), transform.position.y - ((gridSize.x * nodeDiameter / 2f)), transform.position.z);
        Gizmos.DrawLine(startPos, targetPos);

        Gizmos.color = new Color(1, 1, 1, 0.25f);
        for (int i = 1; i < gridSize.y; i++)
        {
            Gizmos.DrawLine(startPos + new Vector3(i*-nodeDiameter,i*(-nodeDiameter/2f),0), targetPos + new Vector3(i*-nodeDiameter, i * (-nodeDiameter / 2f), 0));
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPos + new Vector3(gridSize.y * -nodeDiameter, gridSize.y * (-nodeDiameter / 2f), 0), targetPos + new Vector3(gridSize.y * -nodeDiameter, gridSize.y * (-nodeDiameter / 2f), 0));
        
        startPos = transform.position;
        targetPos = new Vector3(transform.position.x - (gridSize.y * nodeDiameter), transform.position.y - ((gridSize.y * nodeDiameter / 2f)), transform.position.z);
        Gizmos.DrawLine(startPos, targetPos);

        Gizmos.color = new Color(1, 1, 1, 0.25f);
        for (int i = 1; i < gridSize.x; i++)
        {
            Gizmos.DrawLine(startPos + new Vector3(i * nodeDiameter, i * (-nodeDiameter / 2f), 0), targetPos + new Vector3(i * nodeDiameter, i * (-nodeDiameter / 2f), 0));
        }

        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPos + new Vector3(gridSize.x * nodeDiameter, gridSize.x * (-nodeDiameter / 2f), 0), targetPos + new Vector3(gridSize.x * nodeDiameter, gridSize.x * (-nodeDiameter / 2f), 0));

        if (grid != null)
        {
            Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;
            foreach (AStarNode node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (path != null)
                    if (path.Contains(node))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius/2));
            }
        }
    }
}


