using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Subject
{
    public string id { get; set; }
    public string name;
    public List<Task> tasks;
}