[System.Serializable]
public class Task
{
    public string id;
    public string subjectId;
    public string title;
    public string description;
    public string dueDateString;

    // von Anja hinzugefügt:
    public int estimatedEffort; // Energieaufwand in Löffeln (1-5)
    public long dueDateTicks;   // sortieren nach Datum
    public bool isDone;
    public string completedDate; // yyyy-MM-dd, für den heutigen Energieverbrauch
    public bool isImportant;     // 80/20: vom Nutzer als besonders wichtig markiert
}
