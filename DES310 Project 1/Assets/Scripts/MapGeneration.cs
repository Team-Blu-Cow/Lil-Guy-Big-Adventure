using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Tile Maps")]
    [SerializeField] Tilemap tileMapFloor;
    [SerializeField] Tilemap tileMapTerrain;
    
    [Header("Tile Sets")]
    [SerializeField] TileData grassTiles;
    [SerializeField] TileData treeTiles;
    [SerializeField] TileData rockTiles;
    [SerializeField] TileData bridgeTiles;

    [SerializeField] IsoGrid grid;

    [Header("Objects")]
    [SerializeField] Item[] items;
    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject exit;
    [SerializeField] Transform Player;

    [Header("Generation variables")]
    [SerializeField] float lowerBound = 0.1f;
    [SerializeField] float upperBound = 0.7f;
    [SerializeField] float multiplier = 0.75f;

    [Header("transition")]
    [SerializeField] Image transition;

    [Header ("InGame Objects")]
    List<GameObject> placedItems = new List<GameObject>();
    List<GameObject> placedExits = new List<GameObject>();
    List<GameObject> placedEnemies = new List<GameObject>();

    [Header ("Perlin noise")]
    List<Vector2> treeGrid = new List<Vector2>();
    [SerializeField] float radius = 5f;

    [SerializeField] BattleManager battleManager;

    private int avalibleExit;
    private int travelledRegions;
    Direction enterDirection = 0;

    private void Awake()
    {
    }

    public void StartSwap(int dir)
    {
        LeanTween.value(transition.gameObject, a => transition.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 1f), 0.3f).setOnComplete(RenderMap);
        enterDirection = (Direction)dir;
    }

    public void RenderMap()
    {
        battleManager.BattleState = BattleState.FINISHED;
        Vector2 size = new Vector2(grid.gridSize.x, grid.gridSize.y);
        // PoissonDisc(grid.gridSize.x, grid.gridSize.y); // Doesnt work really just did some boofed stuff wiht the perlin noise map to place trees instead

        //Clear the current tileset
        tileMapFloor.ClearAllTiles();
        tileMapTerrain.ClearAllTiles();

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

        // Clear enemies
        battleManager.enemyCombatants.Clear();
        placedEnemies.ForEach(Destroy);
        placedEnemies.Clear();

        // Check if it should be a campfire
        if (travelledRegions >= 3 && Random.Range(0, 100 * (1 / (float)travelledRegions)) < 10)
        {
            //Swap to campfire scene?
            travelledRegions = 0;
            LeanTween.value(transition.gameObject, a => transition.color = a, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0f), 0.2f);
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

        // map gen positons to be used later
        Queue<Vector3> lakePlace = new Queue<Vector3>();

        //Loop through the width of the map
        for (int x = 1; x < size.x - 1; x++)
        {
            //Loop through the height of the map
            for (int y = 1; y < size.y - 1; y++)
            {
                float dx = Mathf.Abs(x - (size.x / 2)), dy = Mathf.Abs(y - (size.y / 2));
                float d = Mathf.Sqrt((dx * dx) + (dy * dy));

                if (map[x, y] > 0.01 * d * d)
                {
                    if (map[x, y] <= lowerBound)
                    {
                        tileMapFloor.SetTile(new Vector3Int(-x, -y, 0), null);
                        lakePlace.Enqueue(new Vector3(x, y, 1));
                    }
                    if (map[x, y] <= upperBound) // Level 1 tile
                    {
                        // Set tile to a random grass
                        tileMapFloor.SetTile(new Vector3Int(-x, -y, 0), grassTiles.tiles[Random.Range(0, 7)]);

                        if (Random.Range(0, 3) == 0)
                            tileMapFloor.SetTile(new Vector3Int(-x, -y, -2), rockTiles.tiles[Random.Range(0, rockTiles.tiles.Length)]); // set under rock on some tiles
                    }
                    else if (map[x, y] <= 0.8f)
                    {
                        // Set a tree tile
                        tileMapFloor.SetTile(new Vector3Int(-x, -y, 0), grassTiles.tiles[Random.Range(0, 5)]);
                        tileMapTerrain.SetTile(new Vector3Int(-x, -y, 0), treeTiles.tiles[Random.Range(0, 2)]);

                        if (Random.Range(0, 2) == 0)
                        {
                            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), new Vector3(-1, 1, 1));
                            tileMapTerrain.SetTransformMatrix(new Vector3Int(-x, -y, 0), matrix);
                        }
                    }
                    else  // Level 2 tile
                    {
                        tileMapFloor.SetTile(new Vector3Int(-x, -y, 2), grassTiles.tiles[Random.Range(7, 11)]);

                    }
                }
            }
        }

        CreateLake(lakePlace, size);
        //CreatePath();
        grid.CreateGrid();

        PlaceExits();
        PlaceItems(size);
        PlaceEnemies(size);

        grid.CreateGrid();

        avalibleExit = 0;
        foreach (GameObject exit in placedExits)
        {
            PathRequestManager.RequestPath(Player.position, exit.transform.position, OnPathFound);
        }

        LeanTween.delayedCall(0.3f, () =>
        {
            if (avalibleExit < 3)
            {
                RenderMap();
            }
            else
            {
                LeanTween.value(transition.gameObject, a => transition.color = a, new Color(0, 0, 0, 1), new Color(0, 0, 0, 0f), 0.3f);
                travelledRegions++;
                battleManager.BattleState = BattleState.START;
                battleManager.CombatantState = CombatantState.START;
                battleManager.CombatUI.deactivateChoiceButtons();
            }
        });
    }

    void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            avalibleExit++;
        }
    }
    void PlaceItems(Vector2 size)
    {
        int[] chances = { 20, 30, 25, 15 };
        int totalRatio = 0;

        foreach (int c in chances)
            totalRatio += c;

        int r = Random.Range(0, totalRatio);

        int iteration = 0; // so you know what to do next
        foreach (int c in chances)
        {
            if ((r -= c) < 0)
                break;
            iteration++;
        }

        int attempts = 30;
        for (int i = 0; i < iteration; i++)
        {
            int n = 0;
            while (n < attempts)
            {
                Vector2 itemPos = new Vector2(Random.Range(0, size.x), Random.Range(0, size.y));
                if (grid.GetNode(new Vector3Int((int)itemPos.x, (int)itemPos.y, 2)).IsTraversable())
                {
                    placedItems.Add(Instantiate(items[Random.Range(0, items.Length)], grid.NodeToWorld(itemPos.x, itemPos.y, 2), new Quaternion(0, 0, 0, 0), transform.GetChild(1)).gameObject);
                    break;
                }
                n++;
            }
        }
    }
    
    void PlaceEnemies(Vector2 size)
    {
        int[] chances = { 10, 40, 30, 20 };
        int totalRatio = 0;

        foreach (int c in chances)
            totalRatio += c;

        int r = Random.Range(0, totalRatio);

        int iteration = 0; // so you know what to do next
        foreach (int c in chances)
        {
            iteration++;
            if ((r -= c) < 0)
                break;
        }

        int attempts = 30;
        for (int i = 0; i < iteration; i++)
        {
            int n = 0;
            while (n < attempts)
            {
                Vector2 enemyPos = new Vector2(Random.Range(0, size.x), Random.Range(0, size.y));
                if (grid.GetNode(new Vector3Int((int)enemyPos.x, (int)enemyPos.y, 2)).IsTraversable())
                {
                    GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], grid.NodeToWorld((int)enemyPos.x, (int)enemyPos.y, 2), new Quaternion(0, 0, 0, 0), transform.GetChild(2)).gameObject;
                    battleManager.enemyCombatants.Add(enemy);
                    enemy.GetComponent<PathFindingUnit>().GridHighLighter = grid.GetComponent<GridHighLighter>();
                    placedEnemies.Add(enemy);
                    break;
                }
                n++;
            }
        }
    }

    void CreateLake(Queue<Vector3> nodeList, Vector2 size)
    {
        while (nodeList.Count > 0)
        {
            Vector3 currentPoint = nodeList.Dequeue();
            if (Random.Range(0, (int)currentPoint.z) < 1)
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
                tileMapFloor.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, 0), null);
                tileMapFloor.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, 2), null);
                tileMapFloor.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, -2), null);  
                tileMapTerrain.SetTile(new Vector3Int((int)-currentPoint.x, (int)-currentPoint.y, 0), null);                            
            }
        }
    }

    void PlaceExits()
    {
        List<Vector2> exitPos = new List<Vector2>();

        // Exit 1 /////////
        int count = 0;
        while (exitPos.Count == 0 && count < grid.gridSize.y)
        {            
            for (int i = 0; i < grid.gridSize.x; i++)
            {
                if (grid.GetNode(new Vector3Int(i, count, 0)).IsTraversable()) 
                {
                    exitPos.Add(new Vector2(i, count));
                }
            }
            count++;
        }
        AddExit(exitPos[Random.Range(0,exitPos.Count)], "Exit" + (int)Direction.TopLeft);
        exitPos.Clear();

        // Exit 2 /////////
        count = grid.gridSize.y-1;
        while (exitPos.Count == 0 && count > 0)
        {            
            for (int i = 0; i < grid.gridSize.x; i++)
            {
                if (grid.GetNode(new Vector3Int(i, count, 0)).IsTraversable()) 
                {
                    exitPos.Add(new Vector2(i, count));
                }
            }
            count--;
        }
        AddExit(exitPos[Random.Range(0,exitPos.Count)], "Exit" + (int)Direction.BottomRight);
        exitPos.Clear();

        // Exit 3 /////////
        count = 0;
        while (exitPos.Count == 0 && count < grid.gridSize.x)
        {
            for (int i = 0; i < grid.gridSize.y; i++)
            {
                if (grid.GetNode(new Vector3Int(count, i, 0)).IsTraversable())
                {
                    exitPos.Add(new Vector2(count, i)); 

                }
            }
            count++;
        }
        AddExit(exitPos[Random.Range(0, exitPos.Count)], "Exit" + (int)Direction.TopRight);
        exitPos.Clear();

        // Exit 4 /////////
        count = grid.gridSize.x-1;
        while (exitPos.Count == 0 && count > 0)
        {
            for (int i = 0; i < grid.gridSize.y; i++)
            {
                if (grid.GetNode(new Vector3Int(count, i, 0)).IsTraversable())
                {
                    exitPos.Add(new Vector2(count, i));
                }
            }
            count--;
        }
        AddExit(exitPos[Random.Range(0, exitPos.Count)], "Exit" + (int)Direction.BottomLeft);
        exitPos.Clear();
    }

    void PlaceBridges(Direction direction, Vector3Int startPos)
    {
        // Create a new exit and set the tag to what has been passed in
        GameObject tempExit = null;

        switch (direction)
        {
            case Direction.TopLeft: // place bridge going -x
                for (int i = 1; i < grid.gridSize.x / 2; i++)
                {
                    if (tileMapFloor.GetTile(new Vector3Int(-i, -startPos.x-1, 0)) == null)
                    {
                        tileMapFloor.SetTile(new Vector3Int(-i, -startPos.x-1, 0), bridgeTiles.tiles[0]);
                    }
                }
                tempExit = Instantiate(exit, grid.NodeToWorld(startPos.x, 0, 2), Quaternion.Euler(0f, 0f, 0f), transform.GetChild(0)).gameObject;

                break;
            case Direction.BottomLeft:// place bridge going +y  
                for (int i = grid.gridSize.y; i > grid.gridSize.y / 2; i--)
                {
                    if (tileMapFloor.GetTile(new Vector3Int(-startPos.y-1, -i, 0)) == null)
                    {
                        tileMapFloor.SetTile(new Vector3Int(-startPos.y-1, -i, 0), bridgeTiles.tiles[0]);
                        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), new Vector3(-1, 1, 1));
                        tileMapFloor.SetTransformMatrix(new Vector3Int(-startPos.y-1, -i, 0), matrix); 
                    }
                }
                tempExit = Instantiate(exit, grid.NodeToWorld(grid.gridSize.y-1, startPos.y, 2), Quaternion.Euler(0f, 180f, 0f), transform.GetChild(0)).gameObject;

                break;
            case Direction.TopRight:// place bridge going -y
                for (int i = 1; i < grid.gridSize.y / 2; i++)
                {
                    if (tileMapFloor.GetTile(new Vector3Int(-startPos.y-1, -i, 0)) == null)
                    {
                        tileMapFloor.SetTile(new Vector3Int(-startPos.y-1, -i, 0), bridgeTiles.tiles[0]);
                        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), new Vector3(-1, 1, 1));
                        tileMapFloor.SetTransformMatrix(new Vector3Int(-startPos.y-1, -i, 0), matrix);
                    }
                }
                tempExit = Instantiate(exit, grid.NodeToWorld(0, startPos.y, 2), Quaternion.Euler(0f, 0f, 0f), transform.GetChild(0)).gameObject;

                break;
            case Direction.BottomRight:// place bridge going +x
                for (int i = grid.gridSize.x; i > grid.gridSize.x / 2; i--)
                {
                    if (tileMapFloor.GetTile(new Vector3Int(-i, -startPos.x-1, 0)) == null)
                    {
                        tileMapFloor.SetTile(new Vector3Int(-i, -startPos.x-1, 0), bridgeTiles.tiles[0]);
                    }
                }
                tempExit = Instantiate(exit, grid.NodeToWorld(startPos.x, grid.gridSize.x-1, 2), Quaternion.Euler(0f, 180f, 0f), transform.GetChild(0)).gameObject;

                break;
            default:
                break;
        }
        
        tempExit.tag = "Exit" + (int)direction;
        placedExits.Add(tempExit);
        
    }

    private void AddExit(Vector2 position, string tag)
    {
        // sets the player to the position where they entered and doesnt spawn an exit on them
        if ((int)enterDirection == tag[4]-48)
        {
            Player.position = grid.NodeToWorld(position.x, position.y - 0.25f, 2);
            return;
        }

        PlaceBridges((Direction)(tag[4] - 48), new Vector3Int((int)position.x, (int)position.y, 0));
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

