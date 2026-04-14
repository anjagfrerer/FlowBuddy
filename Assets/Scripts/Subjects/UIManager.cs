using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject addSubjectPanel;

    [Header("Confirmation Card")]
    public GameObject confirmDeleteCard;
    public Button yesButton;
    public Button noButton;

    //Toast
    [Header("Toast Settings")]
    public GameObject toastPanel;
    public TMP_Text toastText;
    public float toastDuration = 2f;

    private Action onConfirmAction;

    void Start()
    {
        noButton.onClick.AddListener(() => confirmDeleteCard.SetActive(false));
        yesButton.onClick.AddListener(ExecuteConfirm);

        ShowMainPanel();
        confirmDeleteCard.SetActive(false);
    }

    public void RequestConfirmation(Action actionToDo)
    {
        onConfirmAction = actionToDo;
        confirmDeleteCard.SetActive(true);
    }

    private void ExecuteConfirm()
    {
        onConfirmAction?.Invoke();
        confirmDeleteCard.SetActive(false);
    }

    public void OpenAddSubject()
    {
        mainPanel.SetActive(false);
        addSubjectPanel.SetActive(true);
    }

    public void ShowMainPanel()
    {
        addSubjectPanel.SetActive(false);
        confirmDeleteCard.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ShowToast(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ToastRoutine(message));
    }

    private System.Collections.IEnumerator ToastRoutine(string message)
    {
        toastText.text = message;
        toastPanel.SetActive(true);

        CanvasGroup group = toastPanel.GetComponent<CanvasGroup>();
        if (group == null) group = toastPanel.AddComponent<CanvasGroup>();

        group.alpha = 0;
        while (group.alpha < 1)
        {
            group.alpha += Time.deltaTime * 5;
            yield return null;
        }

        yield return new WaitForSeconds(toastDuration);

        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * 2;
            yield return null;
        }

        toastPanel.SetActive(false);
    }
}