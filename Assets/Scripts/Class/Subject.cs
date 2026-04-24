using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Subject
{
    public int id { get; set; }
    public string name;
    public List<TaskData> tasks = new List<TaskData>();
}

[System.Serializable]
public class TaskData
{
    public string taskName;
    public bool isDone;
}