using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskDataManager : MonoBehaviour
{
    private List<Task> tasks;

    public AppData appData;

    private void Awake()
    {
        tasks = DataManager.Instance.appData.tasks;
        
    }

    // CRUD
    // Create
    public void CreateTask(Task task)
    {
        tasks.Add(task);
        DataManager.Instance.SaveData();
    }

    // List
    public List<Task> ListTasks(string subjectId)
    {
        return tasks.FindAll(task => task.subjectId == subjectId);
    }

    // Read
    public Task ReadTask(string id)
    {
        return tasks.Find(t => t.id == id);
    }

    // Update
    public void UpdateTask(string id, string title, string description, string dueDateString)
    {
        Task task = tasks.Find(task => task.id == id);

        // If task doesn't exists
        if (task == null)
        {
            Debug.Log("Update task failed");
            return;
        }

        task.title = title;
        task.description = description;
        task.dueDateString = dueDateString;

        DataManager.Instance.SaveData();
    }

    // Delete

    public void DeleteTask(string id)
    {
        Task taskToRemove = tasks.Find(task => task.id == id);

        if (taskToRemove != null)
        {
            tasks.Remove(taskToRemove);
            DataManager.Instance.SaveData();
        }
    }

}
