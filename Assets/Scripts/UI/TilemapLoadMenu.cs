using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TilemapLoadMenu : MonoBehaviour
{
    public TilemapLoadFileElement tilemapLoadFileElementPrefab;
    public Transform content;
    public Button back;

    private List<GameObject> elements = new List<GameObject>();

    void OnEnable()
    {
        back.onClick.AddListener(Back);
        TileMapTesting.Instance.isInMenu = true;
        SaveSystem.Init();
        DirectoryInfo directoryInfo = new DirectoryInfo(SaveSystem.SAVE_FOLDER);

        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SaveSystem.SAVE_EXTENTION);

        foreach (FileInfo fileInfo in saveFiles)
        {
            TilemapLoadFileElement element = Instantiate(tilemapLoadFileElementPrefab, content);
            element.fileName.text = fileInfo.Name;
            elements.Add(element.gameObject);
        }
    }

    private void Back()
    {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        TileMapTesting.Instance.isInMenu = false;
        back.onClick.RemoveListener(Back);

        foreach(GameObject obj in elements)
        {
            Destroy(obj);         
        }
    }
}
