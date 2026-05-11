using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskListLoader : MonoBehaviour
{
    public GameObject manageTasksPanel;
    public GameObject configureTaskPanel;

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

        manageTasksPanel.SetActive(false);
        configureTaskPanel.SetActive(false);
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
        string selectedSubjectId = DataManager.Instance.selectedSubjectId;

        Subject selectedSubject = DataManager.Instance.appData.subjects.Find(subject => subject.id == selectedSubjectId);

        // Show all tasks of a specific subject
        foreach (var task in DataManager.Instance.appData.tasks)
        {
            if (task.subjectId == selectedSubject.id)
                CreateButton(task.title);
        }
    }

    private void CreateButton(string taskName)
    {
        GameObject buttonObj = Instantiate(taskPrefab, content);

        TMP_Text text = buttonObj.transform.Find("Text").GetComponent<TMP_Text>();
        text.text = taskName;

        // Button button = buttonObj.GetComponent<Button>();
        // button.onClick.RemoveAllListeners();
        // button.onClick.AddListener(() =>
        // {
        //     Debug.Log("Task clicked: " + taskName);
        //     DataManager.Instance.selectedTaskName = taskName; // DataManager melden, welche Task gewählt wurde
        //     if (sceneChanger != null)
        //     {
        //         sceneChanger.Load(SceneID.TaskPage);
        //     }
        //     else
        //     {
        //         Debug.LogError("SceneChanger fehlt in der Szene!");
        //     }
        // });
    }

    private void ClearContent()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
