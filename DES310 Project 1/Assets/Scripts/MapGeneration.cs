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
    [SerializeField] Tilemap tileMap;
    [SerializeField] TileBase[] tiles;
    [SerializeField] IsoGrid grid;

    [Header("Objects")]
    [SerializeField] Item[] items;
    [SerializeField] GameObject exit;
    [SerializeField] Transform Player;

    [Header("Generation variables")]
    [SerializeField] float lowerBound = 0.1f;
    [SerializeField] float upperBound = 0.7f;
    [SerializeField] float multiplier = 0.75f;

    [SerializeField] float radius = 5f;

    List<GameObject> placedItems = new List<GameObject>();
    List<GameObject> placedExits = new List<GameObject>();

    List<Vector2> treeGrid = new List<Vector2>();
    private int avalibleExit;

    private int travelledRegions;
    Direction enterDirection = 0;


    public void RenderMap(int direction)
    {
        Vector2 size = new Vector2(grid.gridSize.x, grid.gridSize.y);
        // PoissonDisc(grid.gridSize.x, grid.gridSize.y); // Doesnt work really just did some boofed stuff wiht the perlin noise map to place trees instead
        enterDirection = (Direction)direction;

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

        float maxRange = 0;
        if (travelledRegions >= 1)
            maxRange = 100 * (1 / (float)travelledRegions);

        // Check if it should be a campfire
        if (travelledRegions >= 3 && Random.Range(0, (int)maxRange) < 10)
        {
            //Swap to campfire scene?
            travelledRegions = 0;
            return;
        }

        // Noise Map
        float[,] map = new float[(int)size.x, (int)size.y];

        // Noise modifiers
        float offsetX = Random.Range(0, 10000);
        float offsetY = Random.Range(0, 10000);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                map[x, y] = Mathf.PerlinNoise((x * multiplier) + offsetX, (y * multiplier) + offsetY);
            }
        }

        int itemsPlaced = 0;
        // map gen positons to be used later
        Queue<Vector3> lakePlace = new Queue<Vector3>();
        Stack<Vector2> exitsX = new Stack<Vector2>();
        Stack<Vector2> exitsY = new Stack<Vector2>();

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
                        lakePlace.Enqueue(new Vector3(x - 1, y - 1, 1));
                    }
                    else if (map[x, y] <= upperBound) // Level 1 tile
                    {
                        // Set tile to a random grass
                        tileMap.SetTile(new Vector3Int(-x - 1, -y - 1, 0), tiles[Random.Range(0, 3)]); 
                        tileMap.SetTile(new Vector3Int(-x - 1, -y - 1, -2), tiles[4]);


                        // If its in the center then add to the exit place
                        if (x == size.x / 2)
                        {
                            if (exitsX.Count >= 2) // remove the last one as we wat the fist and last poitions on that row
                            {
                                exitsX.Pop();
                            }
                            exitsX.Push(new Vector2(x, y)); // add the new position
                        }
                        else if (y == size.x / 2)
                        {
                            if (exitsY.Count >= 2)
                            {
                                exitsY.Pop();
                            }
                            exitsY.Push(new Vector2(x, y));
                        }
                        else if (Random.Range(0, 50) == 0 && itemsPlaced < 3) // Place items at a random rate but no more than 3
                        {
                            placedItems.Add(Instantiate(items[Random.Range(0, items.Length)], FindObjectOfType<Grid>().CellToWorld(new Vector3Int(-x , -y , 2)), new Quaternion(0, 0, 0, 0)).gameObject);
                            itemsPlaced++;
                        }

                    }
                    else if (map[x, y] <= 0.8f)
                    {
                        tileMap.SetTile(new Vector3Int(-x - 1, -y - 1, 0), tiles[5]);
                    }
                    else  // Level 2 tile
                    {
                        tileMap.SetTile(new Vector3Int(-x + 1, -y + 1, 2), tiles[Random.Range(0, 3)]);
                        tileMap.SetTile(new Vector3Int(-x+1, -y+1, 0), tiles[4]);
                    }
                }
            }
        }

        CreateLake(lakePlace, size);
        //CreatePath();
        PlaceExits(enterDirection, exitsX, exitsY);

        grid.CreateGrid();

        avalibleExit = 0;
        foreach (GameObject exit in placedExits)
        {
            PathRequestManager.RequestPath(Player.position, exit.transform.position, OnPathFound);
        }

        LeanTween.delayedCall(0.3f, RemakeMap);
    }

    void RemakeMap()
    {
        if (avalibleExit == 0)
        {
            RenderMap((int)enterDirection);
        }
        else
        {
            travelledRegions++;
        }
    }

    void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            avalibleExit++;
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
                tileMap.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, -2), null);
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
            if (x.Count - 1 == (int)enterDir)
            {
                temp = x.Pop();
                Player.position = grid.CellToWorld(new Vector3Int(-(int)temp.x - 1, -(int)temp.y - 1, 2)) + new Vector3(0, -0.25f, 0);
            }
            else
            {
                temp = x.Pop();
                if (x.Count == 1)
                {
                    GameObject tempExit = Instantiate(exit, grid.CellToWorld(new Vector3Int(-(int)temp.x, -(int)temp.y, 0)), Quaternion.Euler(0f, 180f, 0f), transform).gameObject;
                    tempExit.tag = "Exit0";
                    placedExits.Add(tempExit);
                }
                else
                {
                    GameObject tempExit = Instantiate(exit, grid.CellToWorld(new Vector3Int(-(int)temp.x, -(int)temp.y, 0)), Quaternion.Euler(0f, 0f, 0f), transform).gameObject;
                    tempExit.tag = "Exit1";
                    placedExits.Add(tempExit);
                }
            }           
        }
        while (y.Count > 0)
        {
            Vector2 temp;
            if (y.Count + 1 == (int)enterDir)
            {
                temp = y.Pop();
                Player.position = grid.CellToWorld(new Vector3Int(-(int)temp.x -1, -(int)temp.y-1, 2)) + new Vector3(0, -0.25f, 0);
            }
            else
            {
                temp = y.Pop();
                
                if (y.Count != 1)
                {
                    GameObject tempExit = Instantiate(exit, grid.CellToWorld(new Vector3Int(-(int)temp.x, -(int)temp.y, 0)), Quaternion.Euler(0f, 0f, 0f), transform).gameObject;
                    tempExit.tag = "Exit3";
                    placedExits.Add(tempExit);
                }
                else
                {
                    GameObject tempExit = Instantiate(exit, grid.CellToWorld(new Vector3Int(-(int)temp.x, -(int)temp.y, 0)), Quaternion.Euler(0f, 0f, 0f), transform).gameObject;
                    tempExit.tag = "Exit2";
                    placedExits.Add(tempExit);
                }
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

    void PoissonDisc(int height, int width)
    {
        treeGrid.Clear();
        List<Vector2> activeGrid = new List<Vector2>();

        // Init variables
        int k = 30; // constant for how much to check for a free space
        float r = radius; // Radius

        int cols = (int)(width );
        int rows = (int)(height);

        // Fill grid with initial NULL
        for (int p = 0; p < cols * rows; p++)
        {
            treeGrid.Add(new Vector2(0, 0));
        }

        // Find start position
        int i = (int)(Random.Range(0, cols - 1));
        int j = (int)(Random.Range(0, rows - 1));

        Vector2 pos = new Vector2(10, 10);

        // Add starting point to the grid
        if (i + j * cols < treeGrid.Count)
        {
            treeGrid[i + j * cols] = pos;
            activeGrid.Add(pos);
        }
        else
        {
            pos = new Vector2(0, 0);
            treeGrid[0] = pos;
            activeGrid.Add(pos);
        }

        // Generate new points
        while (activeGrid.Count > 0)
        {
            // choose a random point in the active list
            int index = Random.Range(0, activeGrid.Count);
            pos = activeGrid[index];
            bool found = false;

            // loop for the amount of atempts you want to have
            for (int n = 0; n < k; n++)
            {
                // Choose a random angle to check
                float angle = Random.Range(0, (int)(2.0f * 3.1415f));

                Vector2 sample = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Choose a random magnitude between r and 2r
                float mag = Random.Range(r, (int)(2 * r));

                // Set new point to the offest of the current point
                sample *= mag*10;
                sample += new Vector2((int)pos.x, (int)pos.y);

                // Find what grid space it is in
                int col = (int)(sample.x );
                int row = (int)(sample.y );

                // Check within range and the grid doesnt already have a point assigned to it
                if (col > -1 && row > -1 && col < cols && row < rows && treeGrid[col + row * cols] == new Vector2(0,0))
                {
                    bool ok = true;
                    // Nested loop for all points arount it
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if ((col + x) + (row + y) * cols > -1 && (col + x) + (row + y) * cols < cols)
                            {
                                Vector2 neighbor = treeGrid[(col + x) + (row + y) * cols];
                                //Vector2 neighbor = new Vector2((col + x) , (row + y) * cols);
                                // Find the distance to the new point
                                float d = Mathf.Sqrt(((sample.x - neighbor.x) * (sample.x - neighbor.x)) + ((sample.y - neighbor.y) * (sample.y - neighbor.y)));
                                // if the distance is smaller than the radius then throw it away
                                if (d < r)
                                {
                                    ok = false;
                                }
                            }
                        }
                    }

                    // if the point is out with the radius
                    if (ok)
                    {
                        // found a point
                        found = true;
                        // set the grid to the new pint that has been calulated
                        treeGrid[col + row * cols] = sample;
                        // add the new point to the acitive list
                        activeGrid.Add(sample);
                    }
                }
            }

            if (!found)
            {
                activeGrid.RemoveAt(index);
            }
        }
    }
}

