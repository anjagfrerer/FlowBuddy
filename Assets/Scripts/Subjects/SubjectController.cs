using System.Collections.Generic;
using UnityEngine;

public class SubjectController : MonoBehaviour
{
    public static SubjectController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public List<string> GetAllSubjects()
    {
        return DataManager.Instance.appData.subjects.ConvertAll(s => s.name);
    }

    public void AddSubject(string name)
    {
        DataManager.Instance.AddSubject(name);
    }

    public void DeleteSubject(string name)
    {
        DataManager.Instance.DeleteSubject(name);
    }
}