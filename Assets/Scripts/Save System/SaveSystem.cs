











using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
    public const string SAVE_EXTENTION = "txt";

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private static bool isInit = false;

    public static void Init()
    {
        if (!isInit)
        {
            isInit = true;
            if (!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
        }
    }

    public static void Save(string fileName, string saveString, bool overwrite)
    {
        Init();
        string saveFileName = fileName;
        if (!overwrite)
        {
            int saveNumber = 1;
            while (File.Exists(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENTION))
            {
                saveNumber++;
                saveFileName = saveFileName + "_" + saveNumber;
            }
        }

        File.WriteAllText(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENTION, saveString);
    }

    public static string Load(string fileName)
    {
        Init();
        if (File.Exists(SAVE_FOLDER + fileName)) //+"." + SAVE_EXTENTION
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + fileName ); //+"." + SAVE_EXTENTION
            return saveString;
        }
        else
            return null;
    }

    public static string LoadMostRecent()
    {
        Init();
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);

        FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENTION);
        FileInfo mostRecentFile = null;

        foreach(FileInfo fileInfo in saveFiles)
        {
            if (mostRecentFile == null)
                mostRecentFile = fileInfo;
            else
            {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                    mostRecentFile = fileInfo;
            }
        }

        if (mostRecentFile != null)
        {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        }
        else
            return null;
    }

    public static void SaveObject(object saveObject)
    {
        SaveObject("save", saveObject, false);
    }

    public static void SaveObject(string fileName, object saveObject, bool overwrite)
    {
        Init();
        string json = JsonUtility.ToJson(saveObject);
        Save(fileName, json, overwrite);
    }

    public static TSaveObject LoadMostRecentObject<TSaveObject>()
    {
        Init();
        string saveString = LoadMostRecent();
        if(!string.IsNullOrEmpty(saveString))
        {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        }
        else
            return default(TSaveObject);
    }

    public static TSaveObject LoadObject<TSaveObject>(string fileName)
    {
        Init();
        string saveString = Load(fileName);
        if (!string.IsNullOrEmpty(saveString))
        {
            TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
            return saveObject;
        }
        else
            return default(TSaveObject);
    }
}
