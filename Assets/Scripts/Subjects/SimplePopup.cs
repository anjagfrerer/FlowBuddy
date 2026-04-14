using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SimplePopup : MonoBehaviour
{
    public TMP_Text messageText;
    public Button confirmButton;
    public Button cancelButton;

    public void Show(string message, Action onConfirm)
    {
        messageText.text = message;
        gameObject.SetActive(true);

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() => {
            onConfirm.Invoke();
            gameObject.SetActive(false);
        });

        cancelButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}