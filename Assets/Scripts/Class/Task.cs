using System;
using UnityEngine;

[System.Serializable]
public class Task
{
    public string id;
    public string subjectId;
    public string title;
    public string description;
    public string dueDateString;
<<<<<<< HEAD
=======
    // von Anja hinzugefügt:
    public int estimatedEffort = 30; // Aufwand in Minuten
    public long dueDateTicks;   // sortieren nach Datum
    public bool isDone;
>>>>>>> 38be61affc6ab526dc9cf52541293e9fc58883b8
}
