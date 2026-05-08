using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListManager_TasksPage : MonoBehaviour
{
    public TaskDataManager taskDataManager;

    [Header("UI References")]
    public Button addButton;
    public Transform content;
    public GameObject listItemPrefab;

    [Header("Icons for Edit/Save")]
    public Sprite editIcon;
    public Sprite saveIcon;

    private List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        // if (addButton != null)
        //     addButton.onClick.AddListener(OnAddButtonClicked);

        RefreshList();
    }

    // private void OnAddButtonClicked()
    // {
    //     string taskName = inputField.text.Trim();
    //     if (!IsNameValid(taskName))
    //     {
    //         return;
    //     }
        
    //     // taskDataManager.CreateTask(taskName);
    //     AddListItem(taskName);
    //     inputField.text = "";
    //     FindObjectOfType<UIManager>().ShowToast($"{taskName} added!");
    // }

    private void AddListItem(string taskId, string taskName)
    {
        GameObject newItem = Instantiate(listItemPrefab, content);

        TMP_Text textComponent = newItem.transform.Find("Text").GetComponent<TMP_Text>();
        Button editButton = newItem.transform.Find("EditButton").GetComponent<Button>();
        Button deleteButton = newItem.transform.Find("DeleteButton").GetComponent<Button>();

        textComponent.text = taskName;

        deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(newItem, textComponent.text, taskId));
        //editButton.onClick.AddListener(() => OnEditButtonClicked(newItem, textComponent, editInput, editButton));

        items.Add(newItem);
    }

    // private void OnEditButtonClicked(GameObject item, TMP_Text textComp, TMP_InputField inputComp, Button btn)
    // {
    //     if (!inputComp.gameObject.activeSelf)
    //     {
    //         textComp.gameObject.SetActive(false);
    //         inputComp.gameObject.SetActive(true);
    //         inputComp.text = textComp.text;
    //         btn.image.sprite = saveIcon;
    //         inputComp.ActivateInputField();
    //     }
    //     else
    //     {
    //         string oldName = textComp.text;
    //         string newName = inputComp.text.Trim();

    //         if (IsNameValid(newName, oldName))
    //         {
    //             if (newName != oldName)
    //             {
    //                 DataManager.Instance.EditSubject(oldName, newName);

    //                 textComp.text = newName;

    //                 UIManager ui = Object.FindFirstObjectByType<UIManager>();
    //                 if (ui != null) ui.ShowToast($"Subject renamed to {newName}!");
    //             }

    //             textComp.gameObject.SetActive(true);
    //             inputComp.gameObject.SetActive(false);
    //             btn.image.sprite = editIcon;
    //         }
    //     }
    // }

    private void OnDeleteButtonClicked(GameObject item, string taskName, string taskId)
    {
        UIManager ui = FindObjectOfType<UIManager>();

        if (ui != null)
        {
            ui.RequestConfirmation(() => {
                taskDataManager.DeleteTask(taskId);
                items.Remove(item);
                Destroy(item);
                FindObjectOfType<UIManager>().ShowToast($"{taskName} deleted!");
            });
        }
        else
        {
            Debug.LogError("UIManager wurde in der Szene nicht gefunden!");
        }
    }

    private void RefreshList()
    {
        foreach (GameObject item in items) Destroy(item);
        items.Clear();

        foreach (var task in DataManager.Instance.appData.tasks)
        {
            AddListItem(task.id, task.title);
        }
    }

    // private bool IsNameValid(string newName, string oldName = "")
    // {
    //     if (string.IsNullOrWhiteSpace(newName))
    //     {
    //         Debug.LogWarning("Aufgabe konnte nicht erstellt werden: Leerer Name");
    //         FindObjectOfType<UIManager>().ShowToast($"Empty name!");
    //         return false;
    //     }

    //     if (newName != oldName)
    //     {
    //         bool exists = DataManager.Instance.appData.tasks.Exists(s => s.title.Equals(newName, System.StringComparison.OrdinalIgnoreCase));

    //         if (exists)
    //         {
    //             Debug.LogWarning($"Aufgabe '{newName}' existiert bereits");
    //             FindObjectOfType<UIManager>().ShowToast($"Name already exists");
    //             return false;
    //         }
    //     }

    //     return true;
    // }
}