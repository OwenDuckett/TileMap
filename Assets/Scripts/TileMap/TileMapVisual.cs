using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TileMapVisual : MonoBehaviour
{
    [System.Serializable]
    public struct TileMapUV
    {
        public TileMapSprite TileMapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    public struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }


    [SerializeField] private TileMapUV[] tileMapUVs;

    private Grid<TileMapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<TileMapSprite, UVCoords> uvCoordsDictionary;
    public static TileMapVisual Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        uvCoordsDictionary = new Dictionary<TileMapSprite, UVCoords>();

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        foreach (TileMapUV uv in tileMapUVs)
        {
            uvCoordsDictionary[uv.TileMapSprite] = new UVCoords {
                uv00 = new Vector2(uv.uv00Pixels.x / textureWidth, uv.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(uv.uv11Pixels.x / textureWidth, uv.uv11Pixels.y / textureHeight),
            };
        }
    }

    // Update is called once per frame
    public void SetGrid(TileMap tilemap, Grid<TileMapObject> grid)
    {
        this.grid = grid;
        UpdateVisuals();
        grid.OnGridValueChanged += OnGridValueChanged;
        tilemap.OnLoaded += OnTileMapLoaded;
    }

    private void OnTileMapLoaded(object sender, EventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if(updateMesh)
        {
            updateMesh = false;
            UpdateVisuals();
        }
    }

    private void UpdateVisuals()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                TileMapObject gridObject = grid.GetGridObject(x, y);
                TileMapSprite sprite = gridObject.GetTileMapSprite();
                Vector2 gridValueUV00 , gridValueUV11;
                if(sprite == TileMapSprite.None)
                {
                    gridValueUV00 = Vector2.zero;
                    gridValueUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    UVCoords uVCoords = uvCoordsDictionary[sprite];
                    gridValueUV00 = uVCoords.uv00;
                    gridValueUV11 = uVCoords.uv11;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private void OnGridValueChanged(object sender, Grid<TileMapObject>.OnGridValueChangedEvent e)
    {
        updateMesh = true;
    }

    public TileMapUV[] GetTileMapUVs()
    {
        return tileMapUVs;
    }
}
