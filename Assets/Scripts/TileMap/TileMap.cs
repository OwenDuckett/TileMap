using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap
{
    public event EventHandler OnLoaded;
    public Grid<TileMapObject> grid;


    public class SaveObject
    {
        public TileMapObject.SaveObject[] tileMapTileObjectArray;
        public int width;
        public int height;
        public float cellSize;
    }

    public TileMap(int width, int height, float cellSize, Vector3 originPosition)
    {
        grid = new Grid<TileMapObject>(width, height, cellSize, originPosition, (Grid<TileMapObject> g, int x, int y) => new TileMapObject(g, x, y));
    }

    public void SetTileMapVisual(TileMapVisual tileMapVisual)
    {
        tileMapVisual.SetGrid(this, grid);
    }

    public void SetTileMapSprite(Vector3 worldPosition, TileMapSprite tileMapSprite)
    {
        TileMapObject mapObject = grid.GetGridObject(worldPosition);
        if (mapObject != null)
            mapObject.SetTileMapSprite(tileMapSprite);
    }

    public void SetAllTileMapSprite(TileMapSprite tilemapSprite)
    {
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                SetTileMapSprite(x, y, tilemapSprite);
            }
        }
    }

    public void SetTileMapSprite(int x, int y, TileMapSprite tilemapSprite)
    {
        TileMapObject tilemapObject = grid.GetGridObject(x, y);
        if (tilemapObject != null)
        {
            tilemapObject.SetTileMapSprite(tilemapSprite);
        }
    }

    public void Save()
    {
        List<TileMapObject.SaveObject> tileMapSaveObjects = new List<TileMapObject.SaveObject>();

        for(int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                TileMapObject mapObject = grid.GetGridObject(i, j);
                tileMapSaveObjects.Add(mapObject.Save());
            }
        }

        SaveObject saveObject = new SaveObject
        {
            tileMapTileObjectArray = tileMapSaveObjects.ToArray(),
            width = grid.GetWidth(),
            height = grid.GetHeight(),
            cellSize = grid.GetCellSize()          
        };
        SaveSystem.SaveObject(saveObject);
    }

    public void Save(string fileName, bool overwrite)
    {
        List<TileMapObject.SaveObject> tileMapSaveObjects = new List<TileMapObject.SaveObject>();

        for (int i = 0; i < grid.GetWidth(); i++)
        {
            for (int j = 0; j < grid.GetHeight(); j++)
            {
                TileMapObject mapObject = grid.GetGridObject(i, j);
                tileMapSaveObjects.Add(mapObject.Save());
            }
        }

        SaveObject saveObject = new SaveObject
        {
            tileMapTileObjectArray = tileMapSaveObjects.ToArray(),
            width = grid.GetWidth(),
            height = grid.GetHeight(),
            cellSize = grid.GetCellSize()
        };
        SaveSystem.SaveObject(fileName ,saveObject, overwrite);
    }

    public void Load()
    {
        SaveObject savedMap = SaveSystem.LoadMostRecentObject<SaveObject>();
        grid = new Grid<TileMapObject>(savedMap.width, savedMap.height, savedMap.cellSize, Vector3.zero, (Grid<TileMapObject> g, int x, int y) => new TileMapObject(g, x, y));

        foreach (TileMapObject.SaveObject tile in savedMap.tileMapTileObjectArray)
        {
            TileMapObject mapObject = grid.GetGridObject(tile.x, tile.y);
            mapObject.Load(tile);
        }

        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    public void Load(string fileName)
    {
        SaveObject savedMap = SaveSystem.LoadObject<SaveObject>(fileName);
        grid = new Grid<TileMapObject>(savedMap.width, savedMap.height, savedMap.cellSize, Vector3.zero, (Grid<TileMapObject> g, int x, int y) => new TileMapObject(g, x, y));

        foreach (TileMapObject.SaveObject tile in savedMap.tileMapTileObjectArray)
        {
            TileMapObject mapObject = grid.GetGridObject(tile.x, tile.y);
            mapObject.Load(tile);
        }

        OnLoaded?.Invoke(this, EventArgs.Empty);
    }
}

public class TileMapObject
{
    private TileMapSprite tileMapSprite;
    private Grid<TileMapObject> grid;
    private int x;
    private int y;

    public TileMapObject(Grid<TileMapObject> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public void SetTileMapSprite(TileMapSprite tileMapSprite)
    {
        this.tileMapSprite = tileMapSprite;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString()
    {
        return tileMapSprite.ToString();
    }

    public TileMapSprite GetTileMapSprite()
    {
        return tileMapSprite;
    }

    [System.Serializable]
    public class SaveObject
    {
        public TileMapSprite tileMapSprite;
        public int x;
        public int y;
    }

    public SaveObject Save()
    {
        return new SaveObject
        {
            tileMapSprite = tileMapSprite,
            x = x,
            y = y,
        };
    }

    internal void Load(SaveObject tile)
    {
        tileMapSprite = tile.tileMapSprite;
    }
}

public enum TileMapSprite
{
    None,
    Ground,
    Ice,
    GroundSnow,
    Ground1,  
    Ground2,
    Ground3,
    Ground4,
    Ground5,
    Ground6,
    Ground7,
    Ground8,
    Ground9,
    Ground10,
    Ground11,
    Ground12,
    Ground13,
    Ground14,
    Ground15,
    Ground16,
    Ground17,
    Ground18,
    Ground19,
    Ground20, 
    Ground21,
    Ground22,
    Ground23,
    Ground24,
    Ground25,
    Ground26,
    Ground27,
    Ground28,
    Ground29,
    Ground30,
    Ground31,
    Ground32,
    Ground33,
    Ground34,
    Ground35,
    Ground36,
    Ground37,
    Ground38,
    Ground39,
    Ground40,
    Ground41,
    Ground42,
    Ground43,
    Ground44,
}
