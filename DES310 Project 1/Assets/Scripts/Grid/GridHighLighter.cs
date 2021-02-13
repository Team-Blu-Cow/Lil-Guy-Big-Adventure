using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GridHighLighter : MonoBehaviour
{
    [HideInInspector] public Vector3 mousePos;
    public int overlayZHeight = 1;

    [SerializeField] private Tilemap overlayTileMap;
    [SerializeField] private Tilemap worldTileMap;
    [SerializeField] public IsoGrid grid;
    [SerializeField] private TileBase highlightTile;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color unwalkableColor;
    [SerializeField] private Color selectableColor;

    private Vector3Int previouslySelectedTile = Vector3Int.zero;
    [SerializeField] private Vector3Int test = new Vector3Int();
    public int testMoveSpeed = 3;

    private List<IsoNode> selectableTiles = new List<IsoNode>();

    private InputMaster input;

    private void Awake()
    {
        input = new InputMaster();
        input.PathfinderTestControls.MousePos.performed += ctx => SetCursorPosition(ctx.ReadValue<Vector2>());

        if (grid == null)
            grid = GetComponent<IsoGrid>();
    }

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

    void Update()
    {
        HighlightTileOnMouseOver();
        HighlightSelectableTiles();
    }

    void HighlightTileOnMouseOver()
    {
        IsoNode node = grid.WorldToNode(mousePos);
        Vector3 logicalGridWorldCoords = new Vector3(node.worldPosition.x, node.worldPosition.y, overlayZHeight);
        Vector3Int tileCoordinate = overlayTileMap.WorldToCell(logicalGridWorldCoords);

        if (tileCoordinate != previouslySelectedTile)
        {
            if (selectableTiles.Contains(grid.GetNode(previouslySelectedTile)))
                SetTileColour(tileCoordinate, selectableColor);
            else
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

        }
    }

    // this method exists purely for debug and isn't expected to be used
    public void SetSelectableTilesOnButton()
    {
        SetSelectableTiles(test, testMoveSpeed);
    }

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

        /*foreach (IsoNode node in newSelectableNodes)
        {
            Vector3 logicalGridWorldCoords = new Vector3(node.worldPosition.x, node.worldPosition.y, overlayZHeight);
            Vector3Int tileCoordinate = overlayTileMap.WorldToCell(logicalGridWorldCoords);

            if (tileCoordinate != previouslySelectedTile)
            {
                overlayTileMap.SetTile(tileCoordinate, highlightTile);
                SetTileColour(tileCoordinate, selectableColor);
            }
        }*/

        selectableTiles = newSelectableNodes;
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
