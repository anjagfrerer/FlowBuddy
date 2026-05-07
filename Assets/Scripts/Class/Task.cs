using System;
using UnityEngine;

[System.Serializable]
public class Task
{
    public string id;
    public string subjectId;
    public string name;
    public string description;
    public string dueDateString;
    // von Anja hinzugefügt:
    public int estimatedEffort = 30; // Aufwand in Minuten
    public long dueDateTicks;   // sortieren nach Datum
    public bool isDone;
}
