using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapTesting : MonoBehaviour
{
    public TileSelectButton tileSelectButtonPrefab;
    public Transform buttonTransform;
    public bool isInMenu = false;
    private List<TileSelectButton> tileSelectButtons = new List<TileSelectButton>();
    public static TileMapTesting Instance;

    [SerializeField] private TileMapVisual tileMapVisual;
    private TileMap tileMap;
    private TileMapSprite mapSprite;

    void Start()
    {
        Instance = this;
        tileMap = new TileMap(5, 5, 10f, Vector3.zero);
        tileMap.SetTileMapVisual(tileMapVisual);

        if (tileSelectButtonPrefab != null)
        {
            foreach (TileMapVisual.TileMapUV uv in tileMapVisual.GetTileMapUVs())
            {
                TileSelectButton button = Instantiate(tileSelectButtonPrefab, buttonTransform);
                button.Init(uv);
                button.inputButton.onClick.AddListener(() => { SetSpriteType(button.tileMapSprite); });
            }
        }
    }

    public void SetSpriteType()
    {
        mapSprite = TileMapSprite.None;
        CMDebug.TextPopupMouse(mapSprite.ToString());
    }

    public void SetSpriteType(TileMapSprite tileMapSprite)
    {
        mapSprite = tileMapSprite;
        CMDebug.TextPopupMouse(mapSprite.ToString());
    }

    public void NewTileMap(int x, int y, float cellSize)
    {
        tileMap = null;
        tileMap = new TileMap(x, y, cellSize, Vector3.zero);
        tileMap.SetTileMapVisual(tileMapVisual);
    }

    public void OnLoad()
    {
        tileMap.SetTileMapVisual(tileMapVisual);
    }

    private void Update()
    {
        if (!isInMenu)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 worldPos = UtilsClass.GetMouseWorldPosition();
                tileMap.SetTileMapSprite(worldPos, mapSprite);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mapSprite = TileMapSprite.None;
            CMDebug.TextPopupMouse(mapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mapSprite = TileMapSprite.Ground;
            CMDebug.TextPopupMouse(mapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mapSprite = TileMapSprite.Ice;
            CMDebug.TextPopupMouse(mapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            mapSprite = TileMapSprite.GroundSnow;
            CMDebug.TextPopupMouse(mapSprite.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            tileMap.Save();
            CMDebug.TextPopupMouse("SAVE!");
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            tileMap.Load();
            CMDebug.TextPopupMouse("LOADED!");
        }
    }

    public TileMap GetTileMap()
    {
        return tileMap;
    }

}
