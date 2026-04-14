using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListManager_AddSubjectPage : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public Button addButton;
    public Transform content;
    public GameObject listItemPrefab;

    [Header("Icons for Edit/Save")]
    public Sprite editIcon;
    public Sprite saveIcon;

    private List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        if (addButton != null)
            addButton.onClick.AddListener(OnAddButtonClicked);

        RefreshList();
    }

    private void OnAddButtonClicked()
    {
        string subjectName = inputField.text.Trim();
        if (!IsNameValid(subjectName))
        {
            return;
        }
        DataManager.Instance.AddSubject(subjectName);
        AddListItem(subjectName);
        inputField.text = "";

        var ui = Object.FindFirstObjectByType<UIManager_AddSubjectPage>();
        if (ui != null) ui.ShowToast($"{subjectName} added!");
    }

    private void AddListItem(string subjectName)
    {
        GameObject newItem = Instantiate(listItemPrefab, content);

        TMP_Text textComponent = newItem.transform.Find("Text").GetComponent<TMP_Text>();
        TMP_InputField editInput = newItem.transform.Find("EditInput").GetComponent<TMP_InputField>();
        Button editButton = newItem.transform.Find("EditButton").GetComponent<Button>();
        Button deleteButton = newItem.transform.Find("DeleteButton").GetComponent<Button>();

        textComponent.text = subjectName;
        editInput.gameObject.SetActive(false);

        deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(newItem, textComponent.text));
        editButton.onClick.AddListener(() => OnEditButtonClicked(newItem, textComponent, editInput, editButton));

        items.Add(newItem);
    }

    private void OnEditButtonClicked(GameObject item, TMP_Text textComp, TMP_InputField inputComp, Button btn)
    {
        if (!inputComp.gameObject.activeSelf)
        {
            textComp.gameObject.SetActive(false);
            inputComp.gameObject.SetActive(true);
            inputComp.text = textComp.text;
            btn.image.sprite = saveIcon;
            inputComp.ActivateInputField();
        }
        else
        {
            string oldName = textComp.text;
            string newName = inputComp.text.Trim();

            if (IsNameValid(newName, oldName))
            {
                if (newName != oldName)
                {
                    DataManager.Instance.EditSubject(oldName, newName);
                    textComp.text = newName;

                    var ui = Object.FindFirstObjectByType<UIManager_AddSubjectPage>();
                    if (ui != null) ui.ShowToast("Saved changes!");
                }

                textComp.gameObject.SetActive(true);
                inputComp.gameObject.SetActive(false);
                btn.image.sprite = editIcon;
            }
        }
    }

    private void OnDeleteButtonClicked(GameObject item, string subjectName)
    {
        UIManager_AddSubjectPage ui = Object.FindFirstObjectByType<UIManager_AddSubjectPage>();

        if (ui != null)
        {
            ui.RequestConfirmation(() => {
                DataManager.Instance.DeleteSubject(subjectName);
                items.Remove(item);
                Destroy(item);

                ui.ShowToast($"{subjectName} deleted!");
            });
        }
        else
        {
            Debug.LogError("UIManager_AddSubjectPage wurde in der Szene nicht gefunden!");
        }
    }

    private void RefreshList()
    {
        foreach (GameObject item in items) Destroy(item);
        items.Clear();

        foreach (var subject in DataManager.Instance.appData.subjects)
        {
            AddListItem(subject.name);
        }
    }

    private bool IsNameValid(string newName, string oldName = "")
    {
        var ui = Object.FindFirstObjectByType<UIManager_AddSubjectPage>();

        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Fach konnte nicht erstellt werden: Leerer Name");

            if (ui != null) ui.ShowToast("Empty name!");
            return false;
        }

        if (newName != oldName)
        {
            bool exists = DataManager.Instance.appData.subjects.Exists(s => s.name.Equals(newName, System.StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Debug.LogWarning($"Fach '{newName}' existiert bereits");

                if (ui != null) ui.ShowToast("Name already exists");
                return false;
            }
        }

        return true;
    }
}