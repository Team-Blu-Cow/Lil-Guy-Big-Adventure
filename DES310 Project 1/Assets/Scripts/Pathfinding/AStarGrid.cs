using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    // Public & Serializable Fields ***************************************************************
    [Header("Grid Settings")]
    [SerializeField] public Vector2Int gridSize;

    [Header("Node Settings")]
    public float nodeRadius;
    public LayerMask unwalkableMask;

    [Header("References")]
    public Tilemap tileMap;
    public List<TileData> tileData;

    [Header("Debug Options")]
    public bool onlyDisplayPathGizmos;

    // Private Fields *****************************************************************************
    private Dictionary<TileBase, TileData> dataFromTiles;

    AStarNode[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Getters & Setters **************************************************************************
    public int MaxSize
    {
        get
        {
            return gridSize.x * gridSize.y;
        }
    }


    // Awake Method *******************************************************************************
    void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var _tileData in tileData)
        {
            foreach(var tile in _tileData.tiles)
            {
                dataFromTiles.Add(tile, _tileData);
            }
        }

        InitGrid();
        CreateGrid();
    }

    // Grid Creation Methods **********************************************************************
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
                // TODO: this is hella jank and doesn't support the 
                // ability to move between layers. will probably have
                // to be overhauled later.

                bool walkable = false;

                // check higher layer for tile
                Vector3 worldPoint = NodeToWorld(x, y, 2);
                Vector3Int currentCell = tileMap.WorldToCell(worldPoint);
                if (tileMap.HasTile(currentCell))
                {
                    // if higher layer exists, set tile below (at [x+1,y+1])
                    // to un-walkable and this cell to walkable to allow
                    // objects to pass behind it.
                    walkable = true;
                    grid[x+1, y+1] = new AStarNode(false, NodeToWorld(x+1,y+1,0), new Vector3Int(x+1, y+1, 0));
                }
                else
                {
                    worldPoint = NodeToWorld(x, y, 0);
                    currentCell = tileMap.WorldToCell(worldPoint);

                    if (tileMap.HasTile(currentCell))
                    {
                        TileBase currentTile = tileMap.GetTile(currentCell);
                        if (dataFromTiles.ContainsKey(currentTile))
                            walkable = dataFromTiles[currentTile].walkable;

                    }
                }
                
                if(grid[x,y] == null)
                    grid[x, y] = new AStarNode(walkable, worldPoint, new Vector3Int(x,y,0));
            }
        }
    }

    // Get Neighbors Method ***********************************************************************
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

        return neighbors;
    }

    // Coordinate Conversion Methods **************************************************************
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

    // Debug Gizmo Methods ************************************************************************
    [HideInInspector]
    public List<AStarNode> path;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x + (gridSize.x*nodeDiameter), transform.position.y - ((gridSize.x * nodeDiameter / 2f)), transform.position.z);
        Gizmos.DrawLine(startPos, targetPos);

        if (!onlyDisplayPathGizmos)
        {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            for (int i = 1; i < gridSize.y; i++)
            {
                Gizmos.DrawLine(startPos + new Vector3(i * -nodeDiameter, i * (-nodeDiameter / 2f), 0), targetPos + new Vector3(i * -nodeDiameter, i * (-nodeDiameter / 2f), 0));
            }

            Gizmos.color = Color.white;
        }

        
        Gizmos.DrawLine(startPos + new Vector3(gridSize.y * -nodeDiameter, gridSize.y * (-nodeDiameter / 2f), 0), targetPos + new Vector3(gridSize.y * -nodeDiameter, gridSize.y * (-nodeDiameter / 2f), 0));
        
        startPos = transform.position;
        targetPos = new Vector3(transform.position.x - (gridSize.y * nodeDiameter), transform.position.y - ((gridSize.y * nodeDiameter / 2f)), transform.position.z);
        Gizmos.DrawLine(startPos, targetPos);

        if (!onlyDisplayPathGizmos)
        {
            Gizmos.color = new Color(1, 1, 1, 0.25f);
            for (int i = 1; i < gridSize.x; i++)
            {
                Gizmos.DrawLine(startPos + new Vector3(i * nodeDiameter, i * (-nodeDiameter / 2f), 0), targetPos + new Vector3(i * nodeDiameter, i * (-nodeDiameter / 2f), 0));
            }
            Gizmos.color = Color.white;
        }
        
        Gizmos.DrawLine(startPos + new Vector3(gridSize.x * nodeDiameter, gridSize.x * (-nodeDiameter / 2f), 0), targetPos + new Vector3(gridSize.x * nodeDiameter, gridSize.x * (-nodeDiameter / 2f), 0));

        if(onlyDisplayPathGizmos)
        {
            if(path != null)
            {
                foreach (AStarNode node in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius / 2));
                }
            }
        }
        else
        {
            if (grid != null)
            {
                Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;
                foreach (AStarNode node in grid)
                {
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;
                    if (path != null)
                        if (path.Contains(node))
                            Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius / 2));
                }
            }
        }
        
    }
}


