using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IsoGrid : MonoBehaviour
{
    // Public & Serializable Fields ***************************************************************
    // grid settings
    [HideInInspector, SerializeField] public Vector2Int gridSize;
    [HideInInspector, SerializeField] private Tilemap tileMap;
    [HideInInspector, SerializeField] private Tilemap detailTileMap;
    [HideInInspector, SerializeField] private GridHighLighter highlighter;

    // node settings
    [HideInInspector, SerializeField] private float nodeRadius;
    [HideInInspector, SerializeField] private List<TileData> tileData;

    // debug settings
    [HideInInspector, SerializeField] private bool DisplayGridGizmos;
    [Range(0,1), HideInInspector, SerializeField] private float GridGizmoOpacity = 0.25f;
    [HideInInspector, SerializeField] private bool DisplayNodeGizmos;

    // Private Fields *****************************************************************************
    private Dictionary<TileBase, TileData> dataFromTiles;

    IsoNode[,] grid;

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

        highlighter = gameObject.GetComponent<GridHighLighter>();

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
        grid = new IsoNode[gridSizeX, gridSizeY];

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
                    grid[x+1, y+1] = new IsoNode(false, NodeToWorld(x+1,y+1,0), new Vector3Int(x+1, y+1, 0));
                }
                else
                {
                    worldPoint = NodeToWorld(x, y, 0);
                    currentCell = tileMap.WorldToCell(worldPoint);

                    if (tileMap.HasTile(currentCell))
                    {
                        if (TileHasData(tileMap,currentCell))
                            walkable = dataFromTiles[tileMap.GetTile(currentCell)].walkable;
                        if (TileHasData(detailTileMap, currentCell))
                            walkable = dataFromTiles[detailTileMap.GetTile(currentCell)].walkable;
                    }
                }
                
                if(grid[x,y] == null)
                    grid[x, y] = new IsoNode(walkable, worldPoint, new Vector3Int(x,y,0));
            }
        }
    }

    // Get Methods ********************************************************************************
    public IsoNode GetNode(Vector3Int node)
    {
        if (PointIsInGrid(node))
            return grid[node.x, node.y];
        else
            return null;
    }

    public List<IsoNode> GetNeighbors(IsoNode _node)
    {
        List<IsoNode> neighbors = new List<IsoNode>();

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

    public List<IsoNode> GetWalkableNodesInRange(Vector3 startPos, int range)
    {
        return GetWalkableNodesInRange(WorldToNode(startPos).gridPosition, range);
    }


    public List<IsoNode> GetWalkableNodesInRange(Vector3Int startTile, int range)
    {
        List<IsoNode> output = new List<IsoNode>();
        List<IsoNode> marked = new List<IsoNode>();
        Queue<IsoNode> processing = new Queue<IsoNode>();

        IsoNode startNode = GetNode(startTile);
        if (startNode == null)
            return null;
        startNode.distance = 0;

        processing.Enqueue(startNode);
        
        while (processing.Count > 0)
        {
            IsoNode node = processing.Dequeue();

            if(node.IsTraversable() && node.distance <= range && !marked.Contains(node))
            {
                if (node.IsTraversable() == true)
                    output.Add(node);
                marked.Add(node);

                foreach (IsoNode neighbor in GetNeighbors(node))
                {
                    if (!marked.Contains(neighbor))
                    {
                        neighbor.distance = 1 + node.distance;
                        processing.Enqueue(neighbor);
                    }
                }
            }
        }

        return output;
    }

    public List<IsoNode> GetTargetableNodesInRange(Vector3Int startTile, int range)
    {
        List<IsoNode> output = new List<IsoNode>();
        List<IsoNode> marked = new List<IsoNode>();
        Queue<IsoNode> processing = new Queue<IsoNode>();

        IsoNode startNode = GetNode(startTile);
        if (startNode == null)
            return null;
        startNode.distance = 0;

        processing.Enqueue(startNode);

        while (processing.Count > 0)
        {
            IsoNode node = processing.Dequeue();

            if ((node.IsTraversable() || node.occupied) && node.distance <= range && !marked.Contains(node))
            {
                if (node.IsTraversable() == true || node.occupied)
                    output.Add(node);
                marked.Add(node);

                foreach (IsoNode neighbor in GetNeighbors(node))
                {
                    if (!marked.Contains(neighbor))
                    {
                        neighbor.distance = 1 + node.distance;
                        processing.Enqueue(neighbor);
                    }
                }
            }
        }

        return output;
    }

    // Util Methods *******************************************************************************
    private bool TileHasData(Tilemap map, Vector3Int currentCell)
    {
        TileBase currentTile = map.GetTile(currentCell);
        if (currentTile != null && dataFromTiles.ContainsKey(currentTile))
            return true;
        return false;
    }

    public Vector3 NodeToWorld(float x, float y, float z)
    {
        return new Vector3(
            (transform.position.x) + (x-y) * (nodeDiameter),
            (transform.position.y) - (x+y) * (nodeRadius) - (nodeRadius),
            z
            );
    }

    public IsoNode WorldToNode(Vector3 _worldPos)
    {
        float node_x, node_y;
        int int_x, int_y;

        node_x = (((-_worldPos.y - nodeRadius) / nodeRadius) + (_worldPos.x / nodeDiameter)) / 2;
        node_y = (-_worldPos.y / nodeRadius) - node_x;

        int_x = Mathf.Clamp(Mathf.RoundToInt(node_x),0,gridSizeX-1);
        int_y = Mathf.Clamp(Mathf.RoundToInt(node_y) - 1,0,gridSizeY-1);

        return grid[int_x, int_y];
    }

    public bool PointIsInGrid(Vector3Int point)
    {
        return (point.x >= 0 && point.x < gridSize.x && point.y >= 0 && point.y < gridSize.y);
    }

    public bool IsNodeTraversable(IsoNode node)
    {
        return node.IsTraversable();
    }

    // Debug Gizmo Methods ************************************************************************
    private void OnDrawGizmos()
    {
        // draw horizontal grid lines
        Gizmos.color = Color.white;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x + (gridSize.x*nodeDiameter), transform.position.y - ((gridSize.x * nodeDiameter / 2f)), transform.position.z);
        Gizmos.DrawLine(startPos, targetPos);
        if (DisplayGridGizmos)
        {
            Gizmos.color = new Color(1, 1, 1, GridGizmoOpacity);
            for (int i = 1; i < gridSize.y; i++)
                Gizmos.DrawLine(startPos + new Vector3(i * -nodeDiameter, i * (-nodeDiameter / 2f), 0), targetPos + new Vector3(i * -nodeDiameter, i * (-nodeDiameter / 2f), 0));
            Gizmos.color = Color.white;
        }
        Gizmos.DrawLine(startPos + new Vector3(gridSize.y * -nodeDiameter, gridSize.y * (-nodeDiameter / 2f), 0), targetPos + new Vector3(gridSize.y * -nodeDiameter, gridSize.y * (-nodeDiameter / 2f), 0));
        
        // draw vertical grid lines
        startPos = transform.position;
        targetPos = new Vector3(transform.position.x - (gridSize.y * nodeDiameter), transform.position.y - ((gridSize.y * nodeDiameter / 2f)), transform.position.z);
        Gizmos.DrawLine(startPos, targetPos);
        if (DisplayGridGizmos)
        {
            Gizmos.color = new Color(1, 1, 1, GridGizmoOpacity);
            for (int i = 1; i < gridSize.x; i++)
                Gizmos.DrawLine(startPos + new Vector3(i * nodeDiameter, i * (-nodeDiameter / 2f), 0), targetPos + new Vector3(i * nodeDiameter, i * (-nodeDiameter / 2f), 0));
            Gizmos.color = Color.white;
        }
        Gizmos.DrawLine(startPos + new Vector3(gridSize.x * nodeDiameter, gridSize.x * (-nodeDiameter / 2f), 0), targetPos + new Vector3(gridSize.x * nodeDiameter, gridSize.x * (-nodeDiameter / 2f), 0));

        // draw nodes
        if (grid != null && DisplayNodeGizmos)
        {
            Vector3 worldTopLeft = transform.position - Vector3.right * gridSize.x / 2 + Vector3.up * gridSize.y / 2;
            foreach (IsoNode node in grid)
            {
                Gizmos.color = (node.IsTraversable()) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius / 2));
            }
        }

    }
}


