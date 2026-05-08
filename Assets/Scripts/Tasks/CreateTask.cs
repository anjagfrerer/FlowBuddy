using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTask : MonoBehaviour
{
    public TaskDataManager taskDataManager;
    public TMP_InputField titleInput;
    public TMP_InputField descriptionInput;
    public TMP_InputField dueDateInput;

    private Task newTask;

    private void Start()
    {
        newTask = new Task
        {
            id = System.Guid.NewGuid().ToString(),
            subjectId = DataManager.Instance.appData.subjects
                .Find(s => s.name == DataManager.Instance.selectedSubjectName)?.id
        };
    }

    public void setTitle()
    {
        newTask.title = titleInput.text;
    }

    public void setDescription()
    {
        newTask.description = descriptionInput.text;
    }

    public void setDueDate()
    {
        newTask.dueDateString = dueDateInput.text;
    }

    public void saveTask()
    {
        setTitle();
        setDescription();
        setDueDate();
        Debug.Log(newTask);
        taskDataManager.CreateTask(newTask);
    }
}
