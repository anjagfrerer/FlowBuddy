using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;
public class SortAndFilterTasks : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Toggle toggle_isDone;
    [SerializeField] private Toggle toggle_descending;
    [SerializeField] private TMP_Dropdown sortByTable;
    private string prompt;
    private bool isDone = true;
    private IEnumerable<Task> tasks = new List<Task>();
    private bool descending = false;


    public enum SORTBY
    {
        Date,
        Effort,
        None,
    }
    public SORTBY sortBy = SORTBY.None;

    void Start()
    {
        tasks = DataManager.Instance.appData.tasks.Where(
            t => t.subjectId == DataManager.Instance.selectedSubjectId);

        if(tasks.Count() == 0)
            tasks = DataManager.Instance.appData.tasks;
    }

    public void SetDescending()
    {
        descending = toggle_descending.isOn;
    }
    public void SetIsDone()
    {
        isDone = toggle_isDone.isOn;
    }

    public void SetSearch()
    {
        prompt = input.text;
    }

    public void SetSortBy()
    {
        string text = sortByTable.options[sortByTable.value].text;

        if(text == "Date")
            sortBy = SORTBY.Date;
        else if(text == "Effort")
            sortBy = SORTBY.Effort;
        else
            sortBy = SORTBY.None;
    }

    public List<Task> StartSearchAndLoadTasksToList()
    {
        if (tasks.Count() == 0)
        {
            Debug.LogWarning("No tasks found for this subject! InitializeTaskList before sorting.");
            return new List<Task>();
        }

        return Sort(Filter(tasks)).ToList();

    }
    private IEnumerable<Task> Filter(IEnumerable<Task> tasks)
    {
        IEnumerable<Task> query = tasks;
        if (!isDone)
            query = query.Where(t => t.isDone == false);
        if (!string.IsNullOrWhiteSpace(prompt))
            query = query.Where(t => t.title.Contains(prompt, StringComparison.OrdinalIgnoreCase));

        if(query.Count() == 0) Debug.Log("Query is Empty");
        return query;
    }



    private IEnumerable<Task> Sort(IEnumerable<Task> tasks)
    {
        if (descending)
        {
            if (sortBy == SORTBY.Date)
                return tasks.OrderByDescending(t => t.dueDateString);
            else if (sortBy == SORTBY.Effort)
                return tasks.OrderByDescending(t => t.estimatedEffort);
        }
        else
        {
            if (sortBy == SORTBY.Date)
                return tasks.OrderBy(t => t.dueDateString);
            else if (sortBy == SORTBY.Effort)
                return tasks.OrderBy(t => t.estimatedEffort);
        }
        return tasks;

    }







}
