using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilemapSaveMenu : MonoBehaviour
{
    public InputField fileName;
    public Toggle overwrite;
    public Button saveButton;
    public Button back;
    private TileMap tilemap;

    private void OnEnable()
    {
        saveButton.onClick.AddListener(Save);
        back.onClick.AddListener(Back);
        tilemap = TileMapTesting.Instance.GetTileMap();
        TileMapTesting.Instance.isInMenu = true;
    }

    private void Back()
    {
        this.gameObject.SetActive(false);
        fileName.text = "";
        overwrite.isOn = false;
    }

    private void Save()
    {
        if (tilemap != null)
        {
            if (string.IsNullOrEmpty(fileName.text))
            {
                Debug.LogError("No File Name");
                tilemap.Save();
            }
            else
                tilemap.Save(fileName.text, overwrite.isOn);
        }
        else
        {
            Debug.LogError("Tilemap is null.");
        }
    }

    private void OnDisable()
    {
        saveButton.onClick.RemoveListener(Save);
        back.onClick.RemoveListener(Back);
        TileMapTesting.Instance.isInMenu = false;
    }
}
