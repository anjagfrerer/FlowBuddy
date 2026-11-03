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
    public Toggle importantToggle; // 80/20, optional: ohne Toggle bleibt alles wie bisher

    private Task task;

    private bool isNew = false;

    private const int DefaultEffort = 3; // "Medium"

    // Aufwand in Löffeln statt Minuten (Spoon Theory)
    private void SetupEffortSlider()
    {
        effortInput.minValue = EnergyBudget.MinEffort;
        effortInput.maxValue = EnergyBudget.MaxEffort;
        effortInput.wholeNumbers = true;
    }

    public void OpenForCreate()
    {
        Initialize(null);
    }

    public void Initialize(Task existingTask)
    {
        SetupEffortSlider();

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
            effortInput.value = DefaultEffort;
            if (importantToggle != null) importantToggle.isOn = false;
            
        } else
        {
            // Edit existing task
            isNew = false;

            task = existingTask;
            titleInput.text = existingTask.title;
            descriptionInput.text = existingTask.description;
            dueDateInput.text = existingTask.dueDateString;
            effortInput.value = existingTask.estimatedEffort;
            if (importantToggle != null) importantToggle.isOn = existingTask.isImportant;
        }
        
    }

    private bool AreFieldsValid(string newTitle, string description, string dueDate, string oldTitle = "")
    {
        UIManager uIManager = FindAnyObjectByType<UIManager>();

        if (string.IsNullOrWhiteSpace(newTitle))
        {
            Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leerer Titel");
            uIManager.ShowToast($"Empty title!");
            return false;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leere Beschreibung");
            uIManager.ShowToast($"Empty description!");
            return false;
        }

        if (string.IsNullOrWhiteSpace(dueDate))
        {
            Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leeres Datum");
            uIManager.ShowToast($"Empty due date!");
            return false;
        }

        if (newTitle != oldTitle)
        {
            bool exists = DataManager.Instance.appData.tasks.Exists(s => s.title.Equals(newTitle, System.StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Debug.LogWarning($"Aufgabe '{newTitle}' existiert bereits");
                uIManager.ShowToast($"Title already exists");
                return false;
            }
        }

        return true;
    }

    public void saveTask()
    {
        UIManager uIManager = FindAnyObjectByType<UIManager>();

        if(!AreFieldsValid(titleInput.text, descriptionInput.text, dueDateInput.text, isNew ? "" : task.title))
            return;

        int oldEffort = task.estimatedEffort;
        setFields();

        if (isNew)
            taskDataManager.CreateTask(task);
        else
        {
            taskDataManager.UpdateTask(task.id, task.title, task.description, task.dueDateString, task.estimatedEffort, task.isDone, task.isImportant);
            EnergyBudget.RegisterEffortChange(task, oldEffort);
        }

        configureTaskPanel.SetActive(false);

        if (isNew)
            uIManager.ShowToast($"{titleInput.text} added!");
        else
            uIManager.ShowToast($"{titleInput.text} updated!");

        FindAnyObjectByType<ListManager_TasksPage>().RefreshList();
        FindAnyObjectByType<TaskListLoader>().RefreshList();
    }

    public void setFields()
    {
        task.title = titleInput.text;
        task.description = descriptionInput.text;
        task.dueDateString = dueDateInput.text;
        task.estimatedEffort = (int)effortInput.value;
        task.dueDateTicks = TaskPriority.ParseDueTicks(task.dueDateString);
        if (importantToggle != null) task.isImportant = importantToggle.isOn;
    }
}
