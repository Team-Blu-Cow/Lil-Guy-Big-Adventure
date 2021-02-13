using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public Tilemap tileMap;
    public TileBase tileBase;

    // Start is called before the first frame update
    void Start()
    {
        //tileMap = new Tilemap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void test()
    {

    }

    public void RenderMap()
    {
        int width = 10, height = 10;

        //Clear the map (ensures we dont overlap)
        tileMap.ClearAllTiles();

        float[,] map = new float[width, height];

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                map[x,y] = Mathf.PerlinNoise(0,0);
            }
        }
        
        //Loop through the width of the map
        //for (int x = 0; x < map.GetUpperBound(0); x++)
        //{
        //    //Loop through the height of the map
        //    for (int y = 0; y < map.GetUpperBound(1); y++)
        //    {
        //        if (map[x, y] <= 0.2f) // No tile
        //        {
        //            tileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
        //        }
        //        else if (map[x, y] > 0.2f && map[x, y] <= 0.8f) // Level 1 tile
        //        {
        //            tileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
        //        }
        //        else if (map[x, y] > 0.8f) // Level 2 tile
        //        {
        //            tileMap.SetTile(new Vector3Int(x, y, 0), currentTile);
        //        }
        //    }
        //}
        
        for (int x = 0; x < tileMap.size.x; x++)
        {
            for (int y = 0; y < tileMap.size.y; y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), tileBase);
                
            }
        }
    }
}
