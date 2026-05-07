using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;          // Für den Slider
using TMPro;                   // Für TextMeshProUGUI

public class LearningPackageMainScreen : MonoBehaviour
{
    [Header("Zuweisung aus der Hierarchy")]
    public Transform taskContentArea; // Hier ziehst du das "Content" Objekt rein
    public GameObject taskPrefab;     // Dein Task-UI-Element als Prefab

    [Header("Services")]
    public LearningPackageService packageService;

    [Header("Progress Bar")]
    public Slider progressBar;
    public TextMeshProUGUI progressText; // Optional für "3/10 erledigt"

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Alte Prefabs löschen
        foreach (Transform child in taskContentArea) Destroy(child.gameObject);

        // Paket generieren
        List<Task> dailyTasks = packageService.GenerateDailyPackage();

        // Prefabs instanziieren und befüllen
        foreach (Task t in dailyTasks)
        {
            GameObject go = Instantiate(taskPrefab, taskContentArea);
            TickableTaskItem uiItem = go.GetComponent<TickableTaskItem>();

            if (uiItem != null)
            {
                uiItem.Setup(t);
            }
        }
    }

    public void UpdateProgress(int completedCount, int totalCount)
    {
        if (totalCount == 0)
        {
            progressBar.value = 0;
            if (progressText != null) progressText.text = "0/0";
            return;
        }

        // Berechnung des Prozentwerts (0.0 bis 1.0)
        float progress = (float)completedCount / totalCount;
        progressBar.value = progress;

        // Text-Update (optional)
        if (progressText != null)
            progressText.text = $"{completedCount}/{totalCount}";
    }
}