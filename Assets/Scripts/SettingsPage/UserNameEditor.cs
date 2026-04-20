using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserNameEditor : MonoBehaviour
{
    [SerializeField] private TMP_Text textDisplay;
    [SerializeField] private TMP_InputField textInput;
    [SerializeField] private UserDataManager userDataManager;

    private string current;

    void Start()
    {
        current = DataManager.Instance.appData.user.username;
        textDisplay.text = current;

        textInput.gameObject.SetActive(false);
    }

    public void Editing()
    {
        textInput.text = current;
        textDisplay.gameObject.SetActive(false);
        textInput.gameObject.SetActive(true);
        textInput.Select();
        textInput.ActivateInputField();
    }

    public void CancelEditing()
    {
        textInput.gameObject.SetActive(false);
        textDisplay.gameObject.SetActive(true);
    }

    public void Save()
    {
        current = textInput.text;
        textDisplay.text = current;

        textInput.gameObject.SetActive(false);
        textDisplay.gameObject.SetActive(true);

        userDataManager.setUserName(current);
    }
}
