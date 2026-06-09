using System.IO;
using UnityEngine;

// Test comment
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private string filePath;
    public AppData appData;
    public string selectedSubjectName; //speichert welches subject geklickt wurde
    public string selectedSubjectId;
    public string selectedTaskName;

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
        Debug.Log($"Appdata will be stored in: {filePath}");
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

        if (DataManager.Instance == null)
            Debug.LogError("No datamanager instance found");

        if (DataManager.Instance.appData == null)
            Debug.LogError("No datamanager instance appdata found");
    }

    public void AddSubject(string name)
    {
        if (appData.subjects.Exists(s => s.name == name))
            return;

        appData.subjects.Add(new Subject { id = System.Guid.NewGuid().ToString(), name = name });
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

    public void EditSubject(string oldName, string newName)
    {
        Subject subjectToEdit = appData.subjects.Find(s => s.name == oldName);

        if (subjectToEdit != null)
        {
            if (appData.subjects.Exists(s => s.name == newName && s.name != oldName))
            {
                Debug.LogWarning($"Edit failed: Subject with name '{newName}' already exists.");
                return;
            }

            subjectToEdit.name = newName;

            SaveData();
            Debug.Log($"Subject '{oldName}' was renamed to '{newName}'");
        }
        else
        {
            Debug.LogError($"Edit failed: Subject '{oldName}' not found.");
        }
    }
}
