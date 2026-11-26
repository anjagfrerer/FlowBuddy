using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DueDateInput : MonoBehaviour
{

    [SerializeField]
    TMP_Text dueDateLabel;

    private DateTime dueDate = new();

    void Start()
    {
        dueDate = DateTime.Now.AddDays(7);
        UpdateDueDateLabel();
    }

    /// <summary>
    /// Shall be called upon saving the Task
    /// </summary>
    /// <returns></returns>
    public string GetDate()
    {
        Debug.Log($"{name}:{dueDate.ToString("dd.MM.yyyy")}");
        return dueDate.ToString("dd.MM.yyyy");
    }

public void InitDate(string date)
{
    if(string.IsNullOrEmpty(date)) dueDate = DateTime.Now.AddDays(7);


    else if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out DateTime parsed))
    {
        Debug.LogError($"{name}: Task date '{date}' could not be parsed");
        parsed = DateTime.Today; // fallback for empty or malformed input
        dueDate = parsed;
    }

    UpdateDueDateLabel();
}


    /// <summary>
    /// Shall be called by the DueDate Buttons
    /// </summary>
    /// <param name="days"></param>
    public void changeDueDate(int days)
    {
        try
        {
            dueDate = dueDate.AddDays(days);
        }
        catch (ArgumentOutOfRangeException)
        {
            return;
        }

        UpdateDueDateLabel();
    }


    private void UpdateDueDateLabel()
    {
        dueDateLabel.text = GetDueDateDisplay(dueDate);
    }
    private string GetDueDateDisplay(DateTime date)
    {
        int diff = (date.Date - DateTime.Now).Days;

        string relative = diff switch
        {
            0 => "due today",
            1 => "due tomorrow",
            -1 => "1 day overdue",
            > 1 => $"due in {diff} days",
            _ => $"{-diff} days overdue"
        };

        return $"{date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} ({relative})";
    }


}
