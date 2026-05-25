using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LearningPackageService : MonoBehaviour
{
    public List<Task> GenerateDailyPackage()
    {
        var allTasks = DataManager.Instance.appData.tasks;
        int energyLimit = DataManager.Instance.appData.user.energyLimit;

        // KORREKTUR: Wir filtern hier NICHT nach !isDone. 
        // Solange die Tasks in allTasks existieren, gehören sie zum aktuellen Paket.
        var prioritizedTasks = allTasks
            .OrderBy(t => t.dueDateTicks) 
            .ThenBy(t => t.estimatedEffort) 
            .ToList();

        List<Task> package = new List<Task>();
        int currentEffortSum = 0;

        foreach (var task in prioritizedTasks)
        {
            if (currentEffortSum + task.estimatedEffort <= energyLimit)
            {
                package.Add(task);
                currentEffortSum += task.estimatedEffort;
            }
            else if (package.Count == 0 && task.estimatedEffort > energyLimit)
            {
                package.Add(task);
                break;
            }
        }

        return package;
    }
}