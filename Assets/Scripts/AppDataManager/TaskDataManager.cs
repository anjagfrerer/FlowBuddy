using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

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
        Debug.Log("Getting task list");
        return tasks.FindAll(task => task.subjectId == subjectId);
    }

    // Read
    public Task ReadTask(string id)
    {
        if (id == "")
        {
            Debug.Log("Empty string: No tasks to read");
        } else if (id == null)
        {
            Debug.Log("Null: No tasks to read");
        } else
        {
            Debug.Log("Reading task");
        }
        
        Task task = tasks.Find(t => t.id == id);
        Debug.Log(task.name);

        return tasks.Find(t => t.id == id);
    }

    // Update
    /**public void UpdateTask(string id, string title, string description, string dueDateString)
    {
        Task task = tasks.Find(task => task.id == id);

        // If task doesn't exists
        if (task == null)
        {
            Debug.Log("Update task failed");
            return;
        }

        task.name = name;
        task.description = description;
        task.dueDateString = dueDateString;

        DataManager.Instance.SaveData();
    }**/

    public void UpdateTask(string id, string title, string description, string dueDateString, int effort, bool done)
    {
        Task task = tasks.Find(t => t.id == id);

        if (task == null) return;

        task.name = title;
        task.description = description;
        task.dueDateString = dueDateString;

        task.estimatedEffort = effort;
        task.isDone = done;

        // für die Sortierung
        if (DateTime.TryParse(dueDateString, out DateTime parsedDate))
        {
            task.dueDateTicks = parsedDate.Ticks;
        }

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
 