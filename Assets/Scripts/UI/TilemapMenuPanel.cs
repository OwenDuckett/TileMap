using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilemapMenuPanel : MonoBehaviour
{
    public event EventHandler<OnFileLoadedEvent> OnFileLoaded;
    public class OnFileLoadedEvent : EventArgs
    {
        public string fileName;
    }

    public Button saveButton, loadButton, newButton;
    public GameObject tilemapSaveMenu;
    public GameObject tilemapLoadMenu;
    public GameObject tilemapNewMenu;
    public static TilemapMenuPanel Instance;

    private TilemapSaveMenu tilemapSave;
    private TilemapNewMenu tilemapNew;
    private string loadedFileName;

    void Awake()
    {
        Instance = this;
        saveButton.onClick.AddListener(Save);
        loadButton.onClick.AddListener(Load);
        newButton.onClick.AddListener(NewGrid);

        tilemapSave = tilemapSaveMenu.GetComponent<TilemapSaveMenu>();
        tilemapNew = tilemapNewMenu.GetComponent<TilemapNewMenu>();

        OnFileLoaded += OnFileLoad;
    }

    private void OnFileLoad(object sender, OnFileLoadedEvent e)
    {
        loadedFileName = e.fileName;
    }

    private void NewGrid()
    {
        tilemapNewMenu.SetActive(true);
        tilemapLoadMenu.SetActive(false);
        tilemapSaveMenu.SetActive(false);
    }

    private void Load()
    {
        tilemapLoadMenu.SetActive(true);
        tilemapSaveMenu.SetActive(false);
        tilemapNewMenu.SetActive(false);
    }

    private void Save()
    {
        tilemapSaveMenu.SetActive(true);
        tilemapNewMenu.SetActive(false);
        tilemapLoadMenu.SetActive(false);

        if(tilemapNew.hasFileName)
        {
            tilemapSave.fileName.text = tilemapNew.GetFileName();
            tilemapSave.overwrite.isOn = true;
        }

        if(!string.IsNullOrEmpty(loadedFileName))
        {
            tilemapSave.fileName.text = loadedFileName;
            tilemapSave.overwrite.isOn = true;
        }
    }

    public void TriggerOnFileLoaded(string name)
    {
        if (OnFileLoaded != null)
            OnFileLoaded(this, new OnFileLoadedEvent { fileName = name });
    }
}
