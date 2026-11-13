using System.Collections.Generic;

[System.Serializable]
public class AppData
{
    public User user = new User();
    public List<Subject> subjects = new List<Subject>();
    public List<Task> tasks = new List<Task>();
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int StatusValue = 50;
    public int energyModelVersion; // für die einmalige Umstellung auf Löffel
}