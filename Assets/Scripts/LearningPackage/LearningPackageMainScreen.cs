using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;          
using TMPro;
using System.Linq;

public class LearningPackageMainScreen : MonoBehaviour
{
    public static event Action OnTaskMissed;

    [Header("Hierarchy Assignments")]
    public Transform taskContentArea; 
    public GameObject taskPrefab;     

    [Header("Services")]
    public LearningPackageService packageService;

    [Header("Progress Bar")]
    public Slider progressBar;
    public TextMeshProUGUI progressText; 

    [Header("Toast Notification")]
    public ToastNotification toastWindow;

    private int totalTasksCount = 0;
    private int completedTasksCount = 0;
    private List<Task> currentDailyTasks = new List<Task>(); 
    private int currentPackageValue;
    private bool isCompletingPackage = false;

    void Start()
    {
        StartCoroutine(WaitForDataManagerAndRefresh());
    }

    private System.Collections.IEnumerator WaitForDataManagerAndRefresh()
    {
        while (DataManager.Instance == null || DataManager.Instance.appData == null || DataManager.Instance.appData.tasks == null)
        {
            yield return null;
        }

        RefreshUI();
    }

    private void OnEnable()
    {
        EnergyBudget.OnBudgetChanged += OnBudgetChanged;
        isCompletingPackage = false; // Coroutine bricht beim Deaktivieren ab

        if (DataManager.Instance != null && DataManager.Instance.appData != null && DataManager.Instance.appData.tasks != null)
        {
            RefreshUI();
        }
    }

    private void OnDisable()
    {
        EnergyBudget.OnBudgetChanged -= OnBudgetChanged;
    }

    // Tagesenergie geändert (z.B. "Rest"): Erledigtes still verbuchen, statt das Paket als fertig zu feiern
    private void OnBudgetChanged()
    {
        // Ein laufender Abschluss baut das Paket danach ohnehin mit dem neuen Budget neu
        if (isCompletingPackage) return;

        SettleCompletedTasks();
        RefreshUI();
    }

    private void SettleCompletedTasks()
    {
        List<Task> doneTasks = currentDailyTasks.Where(t => t.isDone).ToList();
        if (doneTasks.Count == 0) return;

        StatusBarManager.OnStatusValueAdd(doneTasks.Sum(t => t.estimatedEffort) * EnergyBudget.StatusPointsPerSpoon);

        foreach (Task doneTask in doneTasks)
        {
            DataManager.Instance.appData.tasks.Remove(doneTask);
        }

        DataManager.Instance.SaveData();
    }

    public void RefreshUI()
    {
        if (DataManager.Instance == null || DataManager.Instance.appData == null || DataManager.Instance.appData.tasks == null) 
            return;

        foreach (Transform child in taskContentArea) Destroy(child.gameObject);

        currentDailyTasks = packageService.GenerateDailyPackage();
        currentPackageValue = currentDailyTasks.Sum(t => t.estimatedEffort);

        totalTasksCount = currentDailyTasks.Count;
        completedTasksCount = 0; 
        
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

        // Nach Änderung der Tagesenergie kann das Paket nur noch aus erledigten Aufgaben bestehen
        if (totalTasksCount > 0 && completedTasksCount == totalTasksCount)
        {
            CompletePackage();
        }
    }

    public void OnTaskStatusChangedInUI(bool isCompleted)
    {
        if (isCompleted) completedTasksCount++;
        else completedTasksCount--;

        completedTasksCount = Mathf.Clamp(completedTasksCount, 0, totalTasksCount);
        UpdateProgressVisuals();

        if (totalTasksCount > 0 && completedTasksCount == totalTasksCount)
        {
            CompletePackage();
        }
    }

    private void CompletePackage()
    {
        if (isCompletingPackage) return;
        isCompletingPackage = true;

        StartCoroutine(GenerateNextPackageRoutine());
    }

    private System.Collections.IEnumerator GenerateNextPackageRoutine()
    {
        // Merken, welches Paket abgeschlossen wurde; RefreshUI kann currentDailyTasks inzwischen ersetzen
        List<Task> finishedPackage = currentDailyTasks;
        int finishedPackageValue = currentPackageValue;

        yield return new WaitForSeconds(0.4f);

        if (toastWindow != null)
        {
            toastWindow.ShowToast("Congratulations! You completed the learning package!", 3.0f);
        }

        yield return new WaitForSeconds(2.6f);

        Debug.Log("Preparing new learning package...");

        // Belohnung und Entfernen zusammen, damit ein abgebrochener Ablauf nichts doppelt vergibt
        StatusBarManager.OnStatusValueAdd(finishedPackageValue * EnergyBudget.StatusPointsPerSpoon);

        foreach (var completedTask in finishedPackage)
        {
            if (!completedTask.isDone)
            {
                OnTaskMissed?.Invoke();
            }
            DataManager.Instance.appData.tasks.Remove(completedTask);
        }
        
        DataManager.Instance.SaveData();
        isCompletingPackage = false;
        RefreshUI();

        if (currentDailyTasks.Count == 0 && EnergyBudget.Remaining == 0 && toastWindow != null)
        {
            toastWindow.ShowToast("You used today's energy. Time to rest!", 3.0f);
        }
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

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            RefreshUI();
        }
    }
}