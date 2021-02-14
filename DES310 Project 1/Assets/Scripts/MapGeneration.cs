using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{   
    public enum Direction
    {
        TopLeft,
        BottomLeft,
        TopRight,        
        BottomRight,
    }

    [Header("Tile set")]
    public Tilemap tileMap;
    public TileBase[] tiles;
    public AStarGrid grid;

    [Header("Objects")]
    public Item[] items;
    public GameObject exit;
    public Transform Player;

    [Header("Generation variables")]
    public float lowerBound = 0.1f;
    public float upperBound = 0.7f;
    public float multiplier = 0.75f;

    List<GameObject> placedItems = new List<GameObject>(); 
    List<GameObject> placedExits = new List<GameObject>(); 

    public void RenderMap()
    {
        Vector2 size = new Vector2(grid.gridSize.x, grid.gridSize.y);
        Direction enterDirection = 0;

        //Clear the current tileset
        tileMap.ClearAllTiles();

        // Clear items
        for (int i = 0; i < placedItems.Count; i++)
        {
            if (placedItems[i].activeSelf)
                Destroy(placedItems[i]);
        }
        placedItems.Clear();
        
        // Clear exits
       
        placedExits.ForEach(Destroy);
        placedExits.Clear();

        // Noise Map
        float[,] map = new float[(int)size.x, (int)size.y];       

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
        // map gen positons to be used later
        Queue<Vector3> lakePlace = new Queue<Vector3>();
        Stack<Vector2> exitsX= new Stack<Vector2>();
        Stack<Vector2> exitsY= new Stack<Vector2>();

        //Loop through the width of the map
        for (int x = 0; x < size.x; x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < size.y; y++)
            {
                float dx = Mathf.Abs(x - (size.x / 2)), dy = Mathf.Abs(y - (size.y / 2));
                float d = Mathf.Sqrt((dx * dx) + (dy * dy));

                if (map[x, y] > 0.01 * d * d)
                {
                    if (map[x, y] <= lowerBound) // No tile
                    {
                        // Clear tile and add to the "lake" queue for use later
                        tileMap.SetTile(new Vector3Int(-x - 1, -y - 1, 0), null);
                        lakePlace.Enqueue(new Vector3(x, y, 1));
                    }
                    else if (map[x, y] <= upperBound) // Level 1 tile
                    {
                        // Set tile to a random grass
                        tileMap.SetTile(new Vector3Int(-x - 1, -y - 1, 0), tiles[Random.Range(0, 3)]);
                       
                        // If its in the center then add to the exit place
                        if (x == size.x/2)
                        {
                            if (exitsX.Count >= 2) // remove the last one as we wat the fist and last poitions on that row
                            {
                                exitsX.Pop();
                            }
                            exitsX.Push(new Vector2(x, y)); // add the new position
                        }
                        else if (y == size.x/2)
                        {
                            if (exitsY.Count >= 2)
                            {
                                exitsY.Pop();
                            }
                            exitsY.Push(new Vector2(x, y));
                        }
                        else if (Random.Range(0, 80) == 0 && itemsPlaced < 3) // Place items at a random rate but no more than 3
                        {
                            placedItems.Add(Instantiate(items[Random.Range(0, items.Length)], FindObjectOfType<Grid>().CellToWorld(new Vector3Int(-x + 1, -y + 1, 2)), new Quaternion(0, 0, 0, 0)).gameObject);
                            itemsPlaced++;
                        }
                        
                    }
                    else  // Level 2 tile
                    {
                        tileMap.SetTile(new Vector3Int(-x + 1, -y + 1, 2), tiles[Random.Range(0, 3)]);
                        tileMap.SetTile(new Vector3Int(-x, -y, 0), tiles[4]);
                    }
                }
            }           
        } 

        CreateLake(lakePlace, size);
        //CreatePath();
        PlaceExits(enterDirection, exitsX, exitsY);
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

    void PlaceExits(Direction enterDir, Stack<Vector2> x, Stack<Vector2> y)
    {
        Grid grid = FindObjectOfType<Grid>();
        
        while (x.Count > 0)
        {
            Vector2 temp;
            if (x.Count-1 == (int)enterDir)
            {
                temp= x.Pop();
                Player.position = grid.CellToWorld(new Vector3Int(-(int)temp.x-1, -(int)temp.y-1, 0));
            }
            else
            {
                temp= x.Pop();
                placedExits.Add(Instantiate(exit, grid.CellToWorld(new Vector3Int(-(int)temp.x, -(int)temp.y,0)), new Quaternion(0,0,0,0), transform).gameObject);
            }           
        }
        while (y.Count > 0)
        {
            Vector2 temp;
            if (y.Count + 1 == (int)enterDir)
            {
                temp = y.Pop();
                Player.position = grid.CellToWorld(new Vector3Int(-(int)temp.x -1, -(int)temp.y-1, 0));
            }
            else
            {
                temp = y.Pop();
                placedExits.Add(Instantiate(exit, grid.CellToWorld(new Vector3Int(-(int)temp.x, -(int)temp.y, 0)), new Quaternion(0,0,0,0), transform).gameObject);
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
                case (int)Direction.TopLeft:
                    if (!path.Contains(new Vector2(currentPoint.x + 1, currentPoint.y)))
                    {
                        currentPoint.x += 1;
                        path.Add(new Vector2(currentPoint.x, currentPoint.y));
                    }
                    break;
                case (int)Direction.TopRight:
                    if (!path.Contains(new Vector2(currentPoint.x - 1, currentPoint.y)))
                    {
                        currentPoint.x -= 1;
                        path.Add(new Vector2(currentPoint.x, currentPoint.y));
                    }
                    break;
                case (int)Direction.BottomLeft:
                    if (!path.Contains(new Vector2(currentPoint.x, currentPoint.y + 1)))
                    {
                        currentPoint.y += 1;
                        path.Add(new Vector2(currentPoint.x, currentPoint.y));
                    }
                    break;
                case (int)Direction.BottomRight:
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
    }
}

