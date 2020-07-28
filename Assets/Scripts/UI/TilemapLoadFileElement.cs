using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilemapLoadFileElement : MonoBehaviour
{
    public Text fileName;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(Load);
    }

    private void Load()
    {
        CMDebug.TextPopupMouse("LOADED!");
        TileMap tilemap = TileMapTesting.Instance.GetTileMap();
        tilemap.Load(fileName.text);
        TileMapTesting.Instance.OnLoad();
        TilemapMenuPanel.Instance.TriggerOnFileLoaded(fileName.text);
    }

    // Update is called once per frame
    void OnDisable()
    {
        button.onClick.RemoveListener(Load);
        Destroy(this);
    }
}
