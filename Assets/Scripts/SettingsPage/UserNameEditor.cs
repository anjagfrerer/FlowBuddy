using TMPro;
using UnityEngine;

public class UserNameEditor : MonoBehaviour
{
    [SerializeField] private TMP_Text textDisplay;
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private UserDataManager userDataManager;

    private string current;

    void Start()
    {
        current = userDataManager.GetUserName();
        textDisplay.text = current;
    }

    public void Editing()
    {
        textInput.text = current;
        textDisplay.gameObject.SetActive(false);
        textInput.Select();
        textInput.ActivateInputField();
    }

    public void CancelEditing()
    {
        textDisplay.gameObject.SetActive(true);
    }

    public void Save()
    {
        current = textInput.text;
        textDisplay.text = current;

        textDisplay.gameObject.SetActive(true);

        userDataManager.setUserName(current);
    }
}
