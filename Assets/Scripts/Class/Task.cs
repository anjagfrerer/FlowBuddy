using System;
using UnityEngine;
[System.Serializable]
public class Task
{
    private int id;
    private string title;
    private string description;
    private DateTime due;
    private Subject subject;
    public Boolean complete{get; set;} = false;

    public int GetId()
    {
        return this.id;
    }
    public void SetId(int id)
    {
        this.id = id;
    }
    public string GetDescription()
    {
        return this.description;
    }
    public void SetDescription(string description)
    {
        this.description = description;
    }
    public string GetTitle()
    {
        return this.title;
    }
    public void SetTitle(string title)
    {
        this.title = title;
    }
    public DateTime GetDue()
    {
        return this.due;
    }
    public void SetDue(DateTime due)
    {
        this.due = due;
    }
    public Subject GetSubject()
    {
        return this.subject;
    }
    public void SetSubject(Subject subject)
    {
        this.subject = subject;
    }
}
