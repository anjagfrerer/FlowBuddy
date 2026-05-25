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
    public Slider effortInput;

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
                    .Find(s => s.name == DataManager.Instance.selectedSubjectName)?.id,
                isDone = false
            };

            titleInput.text = "";
            descriptionInput.text = "";
            dueDateInput.text = "";
            effortInput.value = 30;
            
        } else
        {
            // Edit existing task
            isNew = false;

            task = existingTask;
            titleInput.text = existingTask.title;
            descriptionInput.text = existingTask.description;
            dueDateInput.text = existingTask.dueDateString;
            effortInput.value = existingTask.estimatedEffort;
        }
    }

    public void saveTask()
    {
        setFields();

        if (isNew)
            taskDataManager.CreateTask(task);
        else
            taskDataManager.UpdateTask(task.id, task.title, task.description, task.dueDateString, task.estimatedEffort, task.isDone);
            // Effort and done missing
            
        configureTaskPanel.SetActive(false);

        FindObjectOfType<ListManager_TasksPage>().RefreshList();
        FindObjectOfType<TaskListLoader>().RefreshList();
    }

    public void setFields()
    {
        task.title = titleInput.text;
        task.description = descriptionInput.text;
        task.dueDateString = dueDateInput.text;
        task.estimatedEffort = (int)effortInput.value;
    }
}
