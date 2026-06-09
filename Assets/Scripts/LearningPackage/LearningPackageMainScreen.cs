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
        if (DataManager.Instance != null && DataManager.Instance.appData != null && DataManager.Instance.appData.tasks != null)
        {
            RefreshUI();
        }
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
    }

    public void OnTaskStatusChangedInUI(bool isCompleted)
    {
        if (isCompleted) completedTasksCount++;
        else completedTasksCount--;

        completedTasksCount = Mathf.Clamp(completedTasksCount, 0, totalTasksCount);
        UpdateProgressVisuals();

        if (totalTasksCount > 0 && completedTasksCount == totalTasksCount)
        {
            StatusBarManager.OnStatusValueAdd(currentPackageValue);
            StartCoroutine(GenerateNextPackageRoutine());
        }
    }

    private System.Collections.IEnumerator GenerateNextPackageRoutine()
    {
        yield return new WaitForSeconds(0.4f);

        if (toastWindow != null)
        {
            toastWindow.ShowToast("Congratulations! You completed the learning package!", 3.0f);
        }

        yield return new WaitForSeconds(2.6f);

        Debug.Log("Preparing new learning package...");

        foreach (var completedTask in currentDailyTasks)
        {
            if (!completedTask.isDone)
            {
                OnTaskMissed?.Invoke();
            }
            DataManager.Instance.appData.tasks.Remove(completedTask);
        }
        
        DataManager.Instance.SaveData();
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

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            RefreshUI();
        }
    }
}