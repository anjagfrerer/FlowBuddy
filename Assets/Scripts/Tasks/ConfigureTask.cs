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

    private bool AreFieldsValid(string newTitle, string description, string dueDate, string oldTitle = "")
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leerer Titel");
            FindAnyObjectByType<UIManager>().ShowToast($"Empty title!");
            return false;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leere Beschreibung");
            FindAnyObjectByType<UIManager>().ShowToast($"Empty description!");
            return false;
        }

        if (string.IsNullOrWhiteSpace(dueDate))
        {
            Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leeres Datum");
            FindAnyObjectByType<UIManager>().ShowToast($"Empty due date!");
            return false;
        }

        if (newTitle != oldTitle)
        {
            bool exists = DataManager.Instance.appData.tasks.Exists(s => s.title.Equals(newTitle, System.StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Debug.LogWarning($"Aufgabe '{newTitle}' existiert bereits");
                FindAnyObjectByType<UIManager>().ShowToast($"Title already exists");
                return false;
            }
        }

        return true;
    }

    public void saveTask()
    {
        if(!AreFieldsValid(titleInput.text, descriptionInput.text, dueDateInput.text, isNew ? "" : task.title))
            return;

        setFields();

        if (isNew)
            taskDataManager.CreateTask(task);
        else
            taskDataManager.UpdateTask(task.id, task.title, task.description, task.dueDateString, task.estimatedEffort, task.isDone);
            // Effort and done missing
            
        configureTaskPanel.SetActive(false);

        if (isNew)
            FindAnyObjectByType<UIManager>().ShowToast($"{titleInput.text} added!");
        else
            FindAnyObjectByType<UIManager>().ShowToast($"{titleInput.text} updated!");

        FindAnyObjectByType<ListManager_TasksPage>().RefreshList();
        FindAnyObjectByType<TaskListLoader>().RefreshList();
    }

    public void setFields()
    {
        task.title = titleInput.text;
        task.description = descriptionInput.text;
        task.dueDateString = dueDateInput.text;
        task.estimatedEffort = (int)effortInput.value;
    }
}
