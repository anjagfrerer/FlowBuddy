using System.Collections.Generic;

[System.Serializable]
public class AppData
{
    public User user = new User();
    public List<Subject> subjects = new List<Subject>();
    public List<Task> tasks = new List<Task>();
}