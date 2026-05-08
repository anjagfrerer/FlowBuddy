using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TaskListLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject taskPrefab;

    [Header("Navigation")]
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private SortAndFilterTasks sortAndFilterTasks;

    private void OnEnable()
    {
        if (sceneChanger == null)
        {
            sceneChanger = FindAnyObjectByType<SceneChanger>();
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
        string selectedSubjectId = DataManager.Instance.selectedSubjectId;

        Subject selectedSubject = DataManager.Instance.appData.subjects.Find(subject => subject.id == selectedSubjectId);

        // Show all tasks of a specific subject
        foreach (var task in DataManager.Instance.appData.tasks)
        {
            if (task.subjectId == selectedSubject.id)
                CreateButton(task.title);
        }
    }

    public void LoadTasksFromQuery()
    {
        if (DataManager.Instance == null ||
            DataManager.Instance.appData == null ||
            DataManager.Instance.appData.tasks == null)
        {
            Debug.LogWarning("No tasks found");
            return;
        }

        ClearContent();

        foreach (Task task in sortAndFilterTasks.StartSearchAndLoadTasksToList())
        {
            CreateButton(task.title);
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
