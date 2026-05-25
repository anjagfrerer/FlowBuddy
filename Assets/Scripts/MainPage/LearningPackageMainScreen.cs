using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;          
using TMPro;                   

public class LearningPackageMainScreen : MonoBehaviour
{
    [Header("Zuweisung aus der Hierarchy")]
    public Transform taskContentArea; 
    public GameObject taskPrefab;     

    [Header("Services")]
    public LearningPackageService packageService;

    [Header("Progress Bar")]
    public Slider progressBar;
    public TextMeshProUGUI progressText; 

    private int totalTasksCount = 0;
    private int completedTasksCount = 0;
    private List<Task> currentDailyTasks = new List<Task>(); // Wir merken uns die geladenen Tasks

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in taskContentArea) Destroy(child.gameObject);

        // Paket über den Service holen
        currentDailyTasks = packageService.GenerateDailyPackage();

        totalTasksCount = currentDailyTasks.Count;
        completedTasksCount = 0; 
        
        // Zählen, wie viele bereits erledigt sind (wichtig für Seitenwechsel)
        foreach (var t in currentDailyTasks)
        {
            if (t.isDone) completedTasksCount++;
        }
        
        UpdateProgressVisuals(); 

        foreach (Task t in currentDailyTasks)
        {
            GameObject go = Instantiate(taskPrefab, taskContentArea);
            TickableTaskItem uiItem = go.GetComponent<TickableTaskItem>();

            if (uiItem != null)
            {
                uiItem.Setup(t, this);
            }
        }
    }

    public void OnTaskStatusChangedInUI(bool isCompleted)
    {
        if (isCompleted)
        {
            completedTasksCount++;
        }
        else
        {
            completedTasksCount--;
        }

        completedTasksCount = Mathf.Clamp(completedTasksCount, 0, totalTasksCount);
        UpdateProgressVisuals();

        // --- NEU: Prüfen, ob ALLE Aufgaben des aktuellen Pakets erledigt sind ---
        if (totalTasksCount > 0 && completedTasksCount == totalTasksCount)
        {
            StartCoroutine(GenerateNextPackageRoutine());
        }
    }

    // Wartet kurz, damit der Spieler sieht, dass die Bar voll ist, und lädt dann das neue Paket
    private System.Collections.IEnumerator GenerateNextPackageRoutine()
    {
        yield return new WaitForSeconds(0.8f);

        Debug.Log("Glückwunsch! Paket komplett gelöst. Bereite neues Lernpaket vor...");

        // 1. Die aktuell erledigten Aufgaben endgültig aus den globalen App-Daten entfernen,
        //    damit sie beim nächsten Generieren nicht wieder im Pool landen.
        foreach (var completedTask in currentDailyTasks)
        {
            DataManager.Instance.appData.tasks.Remove(completedTask);
        }
        
        // Daten speichern, damit die Aufgaben auch nach App-Neustart weg sind
        DataManager.Instance.SaveData();

        // 2. Komplett neues Paket generieren und UI frisch aufbauen
        RefreshUI();
    }

    private void UpdateProgressVisuals()
    {
        if (totalTasksCount == 0)
        {
            if (progressBar != null) progressBar.value = 0;
            if (progressText != null) progressText.text = "0/0";
            return;
        }

        float progress = (float)completedTasksCount / totalTasksCount;
        if (progressBar != null) progressBar.value = progress;
        if (progressText != null) progressText.text = $"{completedTasksCount}/{totalTasksCount}";
    }
}