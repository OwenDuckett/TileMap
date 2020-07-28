using GridPathfindingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    [SerializeField] private TileMapPathVisual tilemapVisual;


    private Grid<EmptyGridObject> grid;
    private MovementTilemap tilemap;
    public GridPathfinding gridPathfinding;
   

    void Awake()
    {
        Instance = this;


        int mapWidth = 40;
        int mapHeight = 25;
        float cellSize = 10f;
        Vector3 origin = new Vector3(0, 0);

        grid = new Grid<EmptyGridObject>(mapWidth, mapHeight, cellSize, origin, (Grid<EmptyGridObject> g, int x, int y) => new EmptyGridObject(g, x, y));
        gridPathfinding = new GridPathfinding(origin + new Vector3(1, 1) * cellSize * .5f, new Vector3(mapWidth, mapHeight) * cellSize, cellSize);
        gridPathfinding.RaycastWalkable();

        tilemap = new MovementTilemap(mapWidth, mapHeight, cellSize, origin);
       
    }

    private void Start()
    {
        tilemap.SetTilemapVisual(tilemapVisual);
    }


    public Grid<EmptyGridObject> GetGrid()
    {
        return grid;
    }

    public MovementTilemap GetMovementTilemap()
    {
        return tilemap;
    }
}

public class EmptyGridObject
{

    private Grid<EmptyGridObject> grid;
    private int x;
    private int y;

    public EmptyGridObject(Grid<EmptyGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;

        Vector3 worldPos00 = grid.GetWorldPosition(x, y);
        Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
        Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
        Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

        Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
        Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
        Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
        Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
    }

    public override string ToString()
    {
        return "";
    }
}
