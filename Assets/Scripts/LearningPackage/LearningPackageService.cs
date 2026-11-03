using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LearningPackageService : MonoBehaviour
{
    // Paket = heute erledigte Aufgaben + offene Aufgaben, die in die restliche Tagesenergie passen
    public List<Task> GenerateDailyPackage()
    {
        var allTasks = DataManager.Instance.appData.tasks;
        string today = EnergyBudget.Today;
        int todayBudget = EnergyBudget.TodayBudget;
        int remaining = EnergyBudget.Remaining;
        Debug.Log($"[Service] Berechne Paket: {remaining} von {todayBudget} Löffeln übrig.");

        List<Task> package = allTasks
            .Where(t => t.isDone && t.completedDate == today)
            .ToList();

        var prioritizedTasks = allTasks
            .Where(t => !t.isDone)
            .OrderBy(t => TaskPriority.Tier(t))
            .ThenBy(t => TaskPriority.DueTicks(t))
            .ThenBy(t => t.estimatedEffort)
            .ToList();

        int currentEffortSum = 0;

        foreach (var task in prioritizedTasks)
        {
            if (currentEffortSum + task.estimatedEffort <= remaining)
            {
                package.Add(task);
                currentEffortSum += task.estimatedEffort;
            }
            else if (package.Count == 0 && remaining == todayBudget && todayBudget > 0 && task.estimatedEffort > todayBudget)
            {
                // Größer als das ganze Tageslimit: als Solo-Paket statt überspringen
                package.Add(task);
                break;
            }
        }

        return package;
    }
}
