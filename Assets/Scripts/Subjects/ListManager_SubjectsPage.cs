using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListManager_SubjectsPage : MonoBehaviour
{


    UIManager uIManager;

    [Header("UI References")]
    [SerializeField]
    TMP_InputField inputField;

    [SerializeField]
    Button addButton;
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject listItemPrefab;

    [Header("Select All")]
    [SerializeField] Toggle selectAllToggle;
    bool selectAll = false;

    private readonly List<ListItemSubjectEdit> items = new();

    private void Start()
    {
        if (addButton != null)
            addButton.onClick.AddListener(OnAddButtonClicked);

        RefreshList();

        if (uIManager == null)
        {
            uIManager = FindAnyObjectByType<UIManager>();
        }

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
        uIManager.ShowToast($"{subjectName} added!");
    }

    private void AddListItem(string subjectName)
    {
        ListItemSubjectEdit newItem = Instantiate(listItemPrefab, content).GetComponent<ListItemSubjectEdit>();

        TMP_Text textComponent = newItem.tMP_Text;
        TMP_InputField editInput = newItem.tMP_InputField;
        Button editButton = newItem.editButton;
        Button deleteButton = newItem.deleteButton;

        newItem.toggle.isOn = false;
        textComponent.text = subjectName;
        editInput.gameObject.SetActive(false);

        deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(newItem, textComponent.text));
        editButton.onClick.AddListener(() => OnEditButtonClicked(textComponent, editInput, editButton));

        items.Add(newItem);
    }

    private void OnEditButtonClicked(TMP_Text textComp, TMP_InputField inputComp, Button btn)
    {
        if (!inputComp.gameObject.activeSelf)
        {
            textComp.gameObject.SetActive(false);
            inputComp.gameObject.SetActive(true);
            inputComp.text = textComp.text;
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

                    if (uIManager != null) uIManager.ShowToast($"Subject renamed to {newName}!");
                }

                textComp.gameObject.SetActive(true);
                inputComp.gameObject.SetActive(false);
            }
        }
    }

    public void SelectAll()
    {
        selectAll = !selectAll;
        selectAllToggle.isOn = selectAll;
        items.ForEach(item => item.toggle.isOn = selectAll);
    }

    public void DeleteAllChecked()
    {
        uIManager.RequestConfirmation(() =>
        {
            var i = 0;
            foreach (var item in items)
            {
                if (item.toggle.isOn)
                {
                    i++;
                    Destroy(item);
                    DataManager.Instance.DeleteSubject(item.tMP_Text.text);
                    Debug.Log(name + ": Subject " + item.name + " deleted!");
                }

            }
            RefreshList();
            uIManager.ShowToast($"{i} subjects deleted!");
        });
    }

    private void OnDeleteButtonClicked(ListItemSubjectEdit item, string subjectName)
    {

        if (uIManager != null)
        {
            uIManager.RequestConfirmation(() =>
            {
                DataManager.Instance.DeleteSubject(subjectName);
                items.Remove(item);
                Destroy(item);
                uIManager.ShowToast($"{subjectName} deleted!");
            });
        }
        else
        {
            Debug.LogError("UIManager wurde in der Szene nicht gefunden!");
        }
    }

    private void RefreshList()
    {
        foreach (var item in items) Destroy(item);
        items.Clear();

        foreach (var subject in DataManager.Instance.appData.subjects)
        {
            AddListItem(subject.name);
        }
    }

    private bool IsNameValid(string newName, string oldName = "")
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            Debug.LogWarning("Fach konnte nicht erstellt werden: Leerer Name");
            uIManager.ShowToast($"Empty name!");
            return false;
        }

        if (newName != oldName)
        {
            bool exists = DataManager.Instance.appData.subjects.Exists(s => s.name.Equals(newName, System.StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Debug.LogWarning($"Fach '{newName}' existiert bereits");
                uIManager.ShowToast($"Name already exists");
                return false;
            }
        }

        return true;
    }
}