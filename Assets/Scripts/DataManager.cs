using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public LoadDroneSettings data;

    private string _file = "droneSettings.json";

    public void LoadData()
    {
        data = new LoadDroneSettings();
        string json = ReadFromFile(_file);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(_file, json);
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            SaveData();
            return "";
        }
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}
