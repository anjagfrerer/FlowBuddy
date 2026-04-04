using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Test comment
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private string filePath;
    public AppData appData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, "flowbuddy_data.json");
        LoadData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(appData, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            appData = JsonUtility.FromJson<AppData>(json);
        }
        else
        {
            appData = new AppData();
        }
    }

    public void AddSubject(string name)
    {
        if (appData.subjects.Exists(s => s.name == name))
            return;

        appData.subjects.Add(new Subject { name = name });
        SaveData();
    }

    public void DeleteSubject(string name)
    {
        Subject toRemove = appData.subjects.Find(s => s.name == name);
        if (toRemove != null)
        {
            appData.subjects.Remove(toRemove);
            SaveData();
        }
    }
}
