using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilemapNewMenu : MonoBehaviour
{
    public InputField xInput, yInput, cellSizeInput, fileName;
    public Button confirmButton, cancelButton;
    public bool hasFileName = false;

    private string fileNameSave;

    void OnEnable()
    {
        confirmButton.onClick.AddListener(CreateNew);
        cancelButton.onClick.AddListener(Back);
        TileMapTesting.Instance.isInMenu = true;
        cellSizeInput.text = "10";
        xInput.text = "0";
        yInput.text = "0";
        fileName.text = "File Name";
    }

    private void Back()
    {
        this.gameObject.SetActive(false);
    }

    private void CreateNew()
    {
        int x, y;

        x = int.Parse(xInput.text);
        y = int.Parse(yInput.text);

        float cellSize;

        cellSize = float.Parse(cellSizeInput.text);
        TileMapTesting.Instance.NewTileMap(x, y, cellSize);
        TileMap tilemap = TileMapTesting.Instance.GetTileMap();
        tilemap.SetAllTileMapSprite(TileMapSprite.Ground);
        tilemap.Save(fileName.text, false);
        hasFileName = true;
        fileNameSave = fileName.text;
    }

    public string GetFileName()
    {
        return fileNameSave;
    }

    void OnDisable()
    {
        confirmButton.onClick.RemoveListener(CreateNew);
        cancelButton.onClick.RemoveListener(Back);
        TileMapTesting.Instance.isInMenu = false;
    }
}
