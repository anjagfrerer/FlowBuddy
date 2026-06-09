using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TickableTaskItem : MonoBehaviour
{
    public TextMeshProUGUI subjectText;
    public TextMeshProUGUI titleText;
    public Toggle completeToggle;
    
    [Header("Visuelles Feedback")]
    public Color finishedTextColor = new Color(0.5f, 0.5f, 0.5f, 0.7f); 
    private Color originalTitleColor;
    private Color originalSubjectColor;

    private Task currentTask;
    private LearningPackageMainScreen mainScreen; 

    private void Awake()
    {
        originalTitleColor = titleText.color;
        originalSubjectColor = subjectText.color;
    }

    public void Setup(Task task, LearningPackageMainScreen screen)
    {
        currentTask = task;
        mainScreen = screen;
        titleText.text = task.title;

        completeToggle.onValueChanged.RemoveAllListeners();
        
        completeToggle.isOn = task.isDone;
        
        if (task.isDone)
        {
            ApplyFinishedVisuals();
        }
        else
        {
            ResetVisuals();
        }

        var subject = DataManager.Instance.appData.subjects.Find(s => s.id.ToString() == task.subjectId);
        subjectText.text = subject != null ? subject.name.Substring(0, 1).ToUpper() : "?";

        completeToggle.onValueChanged.AddListener((isOn) => {
            StartCoroutine(DelayedToggleRoutine(isOn));
        });
    }

    private IEnumerator DelayedToggleRoutine(bool isOn)
    {
        yield return new WaitForSeconds(0.2f);
        
        if (currentTask != null)
        {
            currentTask.isDone = isOn;

            if (isOn)
            {
                DataManager.Instance.appData.user.coins += 10;
            }

            DataManager.Instance.SaveData(); 
        }

        if (mainScreen != null)
        {
            mainScreen.OnTaskStatusChangedInUI(isOn);
        }

        if (isOn)
        {
            ApplyFinishedVisuals();
        }
        else
        {
            ResetVisuals();
        }
    }

    private void ApplyFinishedVisuals()
    {
        titleText.color = finishedTextColor;
        subjectText.color = finishedTextColor;
        completeToggle.interactable = true; 
    }

    private void ResetVisuals()
    {
        titleText.color = originalTitleColor;
        subjectText.color = originalSubjectColor;
        completeToggle.interactable = true;
    }
}