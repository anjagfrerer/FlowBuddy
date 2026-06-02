[System.Serializable]
public class Task
{
    public string id;
    public string subjectId;
    public string title;
    public string description;
    public string dueDateString;

    // von Anja hinzugefügt:
    public int estimatedEffort; // Aufwand in Minuten
    public long dueDateTicks;   // sortieren nach Datum
    public bool isDone;
}
