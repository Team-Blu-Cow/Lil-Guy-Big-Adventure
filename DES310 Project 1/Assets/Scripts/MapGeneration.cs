using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [Header("Tile set")]
    public Tilemap tileMap;
    public TileBase[] tiles;
    public AStarGrid grid;

    [Header("Items")]
    public Item[] items;

    [Header("Generation variables")]
    public float lowerBound = 0.1f;
    public float upperBound = 0.7f;
    public float multiplier = 0.75f;

    // Update is called once per frame
    void Update()
    {

    }

    public void RenderMap()
    {
        Vector2 size = new Vector2(grid.gridSize.x, grid.gridSize.y);

        //Clear the current tileset
        tileMap.ClearAllTiles();
        Item[] oldItems = FindObjectsOfType<Item>();
        foreach (Item item in oldItems)
        {
            Destroy(item.gameObject);
        }

        // Noise Map
        float[,] map = new float[(int)size.x, (int)size.y];

        Queue<Vector3> lakePlace = new Queue<Vector3>();

        // Noise modifiers
        float offset = Random.Range(0, 10000);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                map[x, y] = Mathf.PerlinNoise((x * multiplier) + offset, (y * multiplier) + offset);
            }
        }

        int itemsPlaced = 0;
        //Loop through the width of the map
        for (int x = 0; x < size.x; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < size.y; y++)
            {
                if (map[x, y] <= lowerBound) // No tile
                {
                    tileMap.SetTile(new Vector3Int(-x-1, -y-1, 0), null);
                    lakePlace.Enqueue(new Vector3(x, y, 1));
                }
                else if (map[x, y] <= upperBound) // Level 1 tile
                {
                    tileMap.SetTile(new Vector3Int(-x-1, -y-1, 0), tiles[Random.Range(0, 3)]);
                    if (Random.Range(0,100) == 0 && itemsPlaced < 4)
                    {                        
                        Instantiate(items[Random.Range(0,items.Length)], FindObjectOfType<Grid>().CellToWorld(new Vector3Int(-x+1,-y+1,2)), new Quaternion(0,0,0,0));
                        itemsPlaced++;
                    }                    
                }
                else  // Level 2 tile
                {
                    tileMap.SetTile(new Vector3Int(-x + 1, -y + 1, 2), tiles[Random.Range(0, 3)]);
                    tileMap.SetTile(new Vector3Int(-x, -y, 0), tiles[4]);
                }
            }

            CreateLake(lakePlace, size);
            //CreatePath();
        }
    }

        void CreateLake(Queue<Vector3> nodeList, Vector2 size)
        {
            while (nodeList.Count > 0)
            {
                Vector3 currentPoint = nodeList.Dequeue();
                if (Random.Range(0, (int)currentPoint.z) == 0)
                {
                    if (currentPoint.x + 1 < size.x)
                        nodeList.Enqueue(new Vector3(currentPoint.x + 1, currentPoint.y, currentPoint.z + 1f));

                    if (currentPoint.x - 1 > 0)
                        nodeList.Enqueue(new Vector3(currentPoint.x - 1, currentPoint.y, currentPoint.z + 1f));

                    if (currentPoint.y + 1 < size.y)
                        nodeList.Enqueue(new Vector3(currentPoint.x, currentPoint.y + 1, currentPoint.z + 1f));

                    if (currentPoint.y - 1 > 0)
                        nodeList.Enqueue(new Vector3(currentPoint.x, currentPoint.y - 1, currentPoint.z + 1f));
                    //set tile to null
                    tileMap.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, 0), null);
                    tileMap.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, 2), null);
                }
            }
        }

        void CreatePath()
        {
            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(10, 10);
            Vector2 currentPoint = start;

            List<Vector2> path = new List<Vector2>();
            bool pathDone = false;

            while (!pathDone)
            {
                int dir = Random.Range(0, 3);

                switch (dir)
                {
                    case 0:
                        if (!path.Contains(new Vector2(currentPoint.x + 1, currentPoint.y)))
                        {
                            currentPoint.x += 1;
                            path.Add(new Vector2(currentPoint.x, currentPoint.y));
                        }
                        break;
                    case 1:
                        if (!path.Contains(new Vector2(currentPoint.x - 1, currentPoint.y)))
                        {
                            currentPoint.x -= 1;
                            path.Add(new Vector2(currentPoint.x, currentPoint.y));
                        }
                        break;
                    case 2:
                        if (!path.Contains(new Vector2(currentPoint.x, currentPoint.y + 1)))
                        {
                            currentPoint.y += 1;
                            path.Add(new Vector2(currentPoint.x, currentPoint.y));
                        }
                        break;
                    case 3:
                        if (!path.Contains(new Vector2(currentPoint.x, currentPoint.y - 1)))
                        {
                            currentPoint.y -= 1;
                            path.Add(new Vector2(currentPoint.x, currentPoint.y));
                        }
                        break;
                    default:
                        break;
                }

                if (currentPoint == end)
                    pathDone = true;
            }

            int x = 0;
            x += 1;
        }
    }

