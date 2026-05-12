using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigureTask : MonoBehaviour
{
    public TaskDataManager taskDataManager;
    public GameObject configureTaskPanel;
    public TMP_InputField titleInput;
    public TMP_InputField descriptionInput;
    public TMP_InputField dueDateInput;

    private Task task;

    private bool isNew = false;

    public void OpenForCreate()
    {
        Initialize(null);
    }

    public void Initialize(Task existingTask)
    {
        // Create new task
        if (existingTask == null)
        {
            isNew = true;

            task = new Task
            {
                id = System.Guid.NewGuid().ToString(),
                subjectId = DataManager.Instance.appData.subjects
                    .Find(s => s.name == DataManager.Instance.selectedSubjectName)?.id
            };

            titleInput.text = "";
            descriptionInput.text = "";
            dueDateInput.text = "";
        } else
        {
            // Edit existing task
            isNew = false;

            task = existingTask;
            titleInput.text = existingTask.title;
            descriptionInput.text = existingTask.description;
            dueDateInput.text = existingTask.dueDateString;
        }
    }

    public void saveTask()
    {
        setFields();

        if (isNew)
            taskDataManager.CreateTask(task);
        else
            taskDataManager.UpdateTask(task.id, task.title, task.description, task.dueDateString);
            
        configureTaskPanel.SetActive(false);

        FindObjectOfType<ListManager_TasksPage>().RefreshList();
    }

    public void setFields()
    {
        task.title = titleInput.text;
        task.description = descriptionInput.text;
        task.dueDateString = dueDateInput.text;
    }
}
