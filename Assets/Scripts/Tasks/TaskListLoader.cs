using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskListLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject taskPrefab;

    [Header("Navigation")]
    [SerializeField] private SceneChanger sceneChanger;

    private void OnEnable()
    {
        if (sceneChanger == null)
        {
            sceneChanger = Object.FindFirstObjectByType<SceneChanger>();
        }
    }

    private void Start()
    {
        LoadTasks();
    }

    private void LoadTasks()
    {
        ClearContent();

        if (DataManager.Instance == null ||
            DataManager.Instance.appData == null ||
            DataManager.Instance.appData.tasks == null)
        {
            Debug.LogWarning("No tasks found");
            return;
        }


        string selectedSubjectName = DataManager.Instance.selectedSubjectName;
        Debug.Log("Selected subject name: " + selectedSubjectName);
        Subject selectedSubject = DataManager.Instance.appData.subjects.Find(subject => subject.name == selectedSubjectName);
        Debug.Log("Found subject: " + (selectedSubject == null ? "NULL" : selectedSubject.id));

        Debug.Log("Selected subject name: " + selectedSubjectName);
        Debug.Log("Found subject: " + (selectedSubject == null ? "NULL" : selectedSubject.id));
        Debug.Log("Task count: " + DataManager.Instance.appData.tasks.Count);

        foreach (var task in DataManager.Instance.appData.tasks)
        {
            if (task.subjectId == selectedSubject.id)
                CreateButton(task.name);
        }
    }

    private void CreateButton(string taskName)
    {
        GameObject buttonObj = Instantiate(taskPrefab, content);

        TMP_Text text = buttonObj.transform.Find("Text").GetComponent<TMP_Text>();
        text.text = taskName;

        Button button = buttonObj.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Debug.Log("Subject clicked: " + taskName);
            DataManager.Instance.selectedTaskName = taskName; // DataManager melden, welche Task gewählt wurde
            if (sceneChanger != null)
            {
                sceneChanger.Load(SceneID.TaskPage);
            }
            else
            {
                Debug.LogError("SceneChanger fehlt in der Szene!");
            }
        });
    }

    private void ClearContent()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
