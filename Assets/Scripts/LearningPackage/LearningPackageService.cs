using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LearningPackageService : MonoBehaviour
{
    public List<Task> GenerateDailyPackage()
    {
        // Daten holen
        var allTasks = DataManager.Instance.appData.tasks;
        int energyLimit = DataManager.Instance.appData.user.energyLimit;

        // Priorisierung
        // Erst nach Datum, dann nach geringstem Aufwand
        var prioritizedTasks = allTasks
            .Where(t => !t.isDone) // Erledigte ignorieren
            .OrderBy(t => t.dueDateTicks) // Deadline zuerst
            .ThenBy(t => t.estimatedEffort) // Kleinerer Aufwand bei gleicher Deadline
            .ToList();

        // Auswahl-Loop
        List<Task> package = new List<Task>();
        int currentEffortSum = 0;

        foreach (var task in prioritizedTasks)
        {
            // Prüfen, ob der Task noch ins Zeitbudget passt
            if (currentEffortSum + task.estimatedEffort <= energyLimit)
            {
                package.Add(task);
                currentEffortSum += task.estimatedEffort;
            }
            else if (package.Count == 0 && task.estimatedEffort > energyLimit)
            {
                // Wenn ein einzelner Task das Limit sprengt, wird er trotzdem als einziger Task vorgeschlagen
                package.Add(task);
                break;
            }
        }

        return package;
    }
}