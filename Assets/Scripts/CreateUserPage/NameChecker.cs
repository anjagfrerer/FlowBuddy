using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameChecker : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button continueButton;
    public UserDataManager userDataManager;

    void Start()
    {
        continueButton.interactable = false;                      // Anfang: Knopf gesperrt
        nameInput.onValueChanged.AddListener(CheckNameInput);     // Listener einrichten 
    }

    void CheckNameInput(string text)
    {
        continueButton.interactable = !string.IsNullOrWhiteSpace(text);
    }

    public void SaveInput()
    {
        userDataManager.setUserName(nameInput.text);
    }
}