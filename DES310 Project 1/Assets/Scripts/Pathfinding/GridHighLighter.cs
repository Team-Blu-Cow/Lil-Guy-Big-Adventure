using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHighLighter : MonoBehaviour
{
    [HideInInspector] public Vector3 mousePos;
    public int overlayZHeight = 1;

    [SerializeField] private Tilemap overlayTileMap;
    [SerializeField] private Tilemap worldTileMap;
    [SerializeField] private IsoGrid grid;
    [SerializeField] private TileBase highlightTile;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color unwalkableColor;

    private Vector3Int previouslySelectedTile;
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
        HilightTileOnMouseOver();
    }

    void HilightTileOnMouseOver()
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
            
        }
    }

    void SetTileColour(Vector3Int tile, Color colour)
    {
        overlayTileMap.SetTileFlags(tile, TileFlags.None);
        overlayTileMap.SetColor(tile, colour);
        overlayTileMap.SetTileFlags(tile, TileFlags.LockColor);
    }

}
