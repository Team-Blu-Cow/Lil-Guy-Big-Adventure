using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GridHighLighter : MonoBehaviour
{
    // Public & Serializable Fields ***************************************************************
    [HideInInspector] public Vector3 mousePos;
    public int overlayZHeight = 1;

    [Header("TileMaps/Grid Data")]
    [SerializeField] private Tilemap overlayTileMap;
    [SerializeField] private Tilemap worldTileMap;
    [SerializeField] public IsoGrid grid { get; private set; }

    [Header("Overlay Colours")]
    [SerializeField] private Color baseColor        = Color.white;
    [SerializeField] private Color unwalkableColor  = Color.red;
    [SerializeField] private Color selectableColor  = new Color(0, 255, 194, 255);

    [Header("Debug Values")]
    [SerializeField] private Vector3Int test = new Vector3Int();
    [SerializeField] private int testMoveSpeed = 3;

    [Header("Path Rendering Data")]
    [SerializeField] public Vector3[] path;
    [SerializeField] private GameObject pathNodePrefab;

    // Private Fields *****************************************************************************
    private List<IsoNode> selectableTiles = new List<IsoNode>();
    private Vector3Int previouslySelectedTile = Vector3Int.zero;

    private InputMaster input;

    private List<GameObject> nodes;

    private TileBase highlightTile;
    private Sprite[] sprites;

    // Awake Method *******************************************************************************
    private void Awake()
    {
        input = new InputMaster();
        input.PathfinderTestControls.MousePos.performed += ctx => SetCursorPosition(ctx.ReadValue<Vector2>());

        if (grid == null)
            grid = GetComponent<IsoGrid>();

        highlightTile = Resources.Load<TileBase>("Tile Palletes/Assets/tile_overlay");

        sprites = Resources.LoadAll<Sprite>("Sprites/tile_overlay_arrow");
    }

    // Input System Methods ***********************************************************************
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void SetCursorPosition(Vector2 pos)
    {
        Vector2 worldPos;
        worldPos = Camera.main.ScreenToWorldPoint(pos);
        mousePos = new Vector3(worldPos.x, worldPos.y, overlayZHeight);
    }

    // Update Method ******************************************************************************
    void Update()
    {
        HighlightTileOnMouseOver();
        HighlightSelectableTiles();
    }

    // Tile Highlighting Methods ******************************************************************
    void HighlightTileOnMouseOver()
    {
        IsoNode node = grid.WorldToNode(mousePos);
        Vector3 logicalGridWorldCoords = new Vector3(node.worldPosition.x, node.worldPosition.y, overlayZHeight);
        Vector3Int tileCoordinate = overlayTileMap.WorldToCell(logicalGridWorldCoords);

        if (tileCoordinate != previouslySelectedTile)
        {
            overlayTileMap.SetTile(previouslySelectedTile, null);

            //if(worldTileMap.HasTile(new Vector3Int(tileCoordinate.x,tileCoordinate.y,0)))
            //{
                overlayTileMap.SetTile(tileCoordinate, highlightTile);

                if (node.walkable)
                    SetTileColour(tileCoordinate, baseColor);
                else
                    SetTileColour(tileCoordinate, unwalkableColor);

                previouslySelectedTile = tileCoordinate;
            //}

            if(selectableTiles.Contains(grid.GetNode(node.gridPosition)))
            {
                //Debug.Log("selectable tile");
                GetPath(selectableTiles[0].worldPosition, node.worldPosition);
            }
        }
    }

    void HighlightSelectableTiles()
    {
        foreach (IsoNode node in selectableTiles)
        {
            Vector3 logicalGridWorldCoords = new Vector3(node.worldPosition.x, node.worldPosition.y, overlayZHeight);
            Vector3Int tileCoordinate = overlayTileMap.WorldToCell(logicalGridWorldCoords);

            if (tileCoordinate != previouslySelectedTile)
            {
                overlayTileMap.SetTile(tileCoordinate, highlightTile);
                SetTileColour(tileCoordinate, selectableColor);
            }
        }
    }

    // Set Selectable Tiles Method ****************************************************************
    public void SetSelectableTiles(IsoNode node, int moveSpeed)
    {
        SetSelectableTiles(node.gridPosition, moveSpeed);
    }

    public void SetSelectableTiles(Vector3Int startPos, int moveSpeed)
    {
        if (startPos == null)
        {
            selectableTiles = new List<IsoNode>();
            return;
        }

        List<IsoNode> newSelectableNodes;

        newSelectableNodes = grid.GetWalkableNodesInRange(startPos, moveSpeed);
        if (newSelectableNodes == null)
            return;

        foreach (IsoNode node in selectableTiles)
        {
            if (!newSelectableNodes.Contains(node))
            {
                Vector3 logicalGridWorldCoords = new Vector3(node.worldPosition.x, node.worldPosition.y, overlayZHeight);
                Vector3Int tileCoordinate = overlayTileMap.WorldToCell(logicalGridWorldCoords);

                overlayTileMap.SetTile(tileCoordinate, null);

            }

        }

        selectableTiles = newSelectableNodes;
    }

    public void ClearSelectableTiles()
    {
        foreach(IsoNode node in selectableTiles)
        {
            Vector3 logicalGridWorldCoords = new Vector3(node.worldPosition.x, node.worldPosition.y, overlayZHeight);
            Vector3Int tileCoordinate = overlayTileMap.WorldToCell(logicalGridWorldCoords);

            overlayTileMap.SetTile(tileCoordinate, null);
        }

        selectableTiles = new List<IsoNode>();
    }

    // Path Rendering Methods *********************************************************************
    public void GetPath(Vector3 startPos, Vector3 endPos)
    {
        PathRequestManager.RequestPath(startPos, endPos, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;

            nodes = new List<GameObject>();

            // create path objects and assign sprites


        }
    }

    // Utility Methods ****************************************************************************
    // this method exists purely for debugging and 
    // isn't expected to be used in production code
    public void SetSelectableTilesOnButton()
    {
        SetSelectableTiles(test, testMoveSpeed);
    }

    void SetTileColour(Vector3Int tile, Color colour)
    {
        overlayTileMap.SetTileFlags(tile, TileFlags.None);
        overlayTileMap.SetColor(tile, colour);
        overlayTileMap.SetTileFlags(tile, TileFlags.LockColor);
    }

    public bool IsTileSelectable(IsoNode node)
    {
        return selectableTiles.Contains(node);
    }

}
