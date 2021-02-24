using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MaskTileController : MonoBehaviour
{
    [HideInInspector] public MaskedTile tile;
    [HideInInspector] public Vector3Int tilePos;
    [HideInInspector] public Sprite sprite;
    [HideInInspector] public Tilemap tilemap;

    private PolygonCollider2D polygonCollider2D;
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> simplifiedPoints = new List<Vector2>();

    private const float hideAlpha = 0.3f;
    private float targetAlpha = 1;
    private float alpha = 1;

    private bool mouseOver = false;

    private void Start()
    {
        polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        polygonCollider2D.isTrigger = true;
        UpdatePolygonCollider2D();
        Tilemap tilemap1 = PathRequestManager.GetGrid().TileMap;
        Tilemap tilemap2 = PathRequestManager.GetGrid().DetailTileMap;

        MaskedTile testTile1 = tilemap1.GetTile(tilePos) as MaskedTile;
        if(testTile1 != null)
        {
            tilemap = tilemap1;
            Debug.Log("tilemap1");
        }
        else
        {
            tilemap = tilemap2;
            Debug.Log("tilemap2");
        }
    }

    public void UpdatePolygonCollider2D(float tolerance = 0.005f)
    {
        polygonCollider2D.pathCount = sprite.GetPhysicsShapeCount();
        for (int i = 0; i < polygonCollider2D.pathCount; i++)
        {
            sprite.GetPhysicsShape(i, points);
            LineUtility.Simplify(points, tolerance, simplifiedPoints);
            polygonCollider2D.SetPath(i, simplifiedPoints);
        }
    }

    private void Update()
    {
        if (mouseOver)
            targetAlpha = hideAlpha;
        else
            targetAlpha = 1;

        alpha = Mathf.Lerp(alpha, targetAlpha, 0.05f);
        SetTileColour(new Color(1, 1, 1, alpha));
    }

    void SetTileColour(Color colour)
    {
        tilemap.SetTileFlags(tilePos, TileFlags.None);
        tilemap.SetColor(tilePos, colour);
        tilemap.SetTileFlags(tilePos, TileFlags.LockColor);

        /*tile.flags = TileFlags.None;
        tile.color = colour;
        tile.flags = TileFlags.LockColor;*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Cursor"))
        {
            mouseOver = true;
            Debug.Log("trigger entered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cursor"))
        {
            mouseOver = false;
            Debug.Log("trigger exited");
        }
    }
}
