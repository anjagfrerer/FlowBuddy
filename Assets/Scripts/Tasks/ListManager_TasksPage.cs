using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ListManager_TasksPage : MonoBehaviour
{
    UIManager uiManager;

    [SerializeField]
    private TaskDataManager taskDataManager;
    [SerializeField]
    private ConfigureTask configureTaskPanel;

    [Header("UI References")]
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject listItemPrefab;

    [SerializeField]
    GameObject addListItemPrefab;

    [Header("Select All")]
    [SerializeField] Toggle selectAllToggle;
    bool selectAll = false;

    private readonly List<ListItemEdit> items = new();

    private void Start()
    {
        RefreshList();

        if(uiManager == null)
        {
            uiManager = FindAnyObjectByType<UIManager>();
        }
    }

    private void AddListItem(string taskId, string taskName)
    {
        ListItemEdit newItem = Instantiate(listItemPrefab, content).GetComponent<ListItemEdit>();

        TMP_Text textComponent = newItem.tMP_Text;
        TMP_InputField editInput = newItem.tMP_InputField;
        Button editButton = newItem.editButton;
        Button deleteButton = newItem.deleteButton;

        if(newItem.toggle)
            newItem.toggle.isOn = false;
        textComponent.text = taskName;
        editInput.gameObject.SetActive(false);

        deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(newItem, textComponent.text, taskId));

        editButton.onClick.AddListener(() =>
        {
            DataManager.Instance.selectedTaskName = taskName;
            Task selectedTask = DataManager.Instance.appData.tasks.Find(task => task.title == taskName);

            configureTaskPanel.gameObject.SetActive(true);
            configureTaskPanel.GetComponent<ConfigureTask>().Initialize(selectedTask);
            
            OnEditButtonClicked(selectedTask);
        });

        items.Add(newItem);
    }

    private void OnEditButtonClicked(Task selectedTask)
    {

    }

    public void SelectAll()
    {
        selectAll = !selectAll;
        selectAllToggle.isOn = selectAll;
        items.ForEach(item => item.toggle.isOn = selectAll);
    }

    public void DeleteAllChecked()
    {
        uiManager.RequestConfirmation(() =>
        {
            var i = 0;
            foreach (var item in items)
            {
                if (item.toggle.isOn)
                {
                    i++;
                    DataManager.Instance.DeleteSubject(item.tMP_Text.text);
                    Debug.Log(name + ": Subject " + item.name + " deleted!");
                }

            }
            RefreshList();
            uiManager.ShowToast($"{i} subjects deleted!");
        });
    }

    private void OnDeleteButtonClicked(ListItemEdit item, string taskName, string taskId)
    {

        if (uiManager != null)
        {
            uiManager.RequestConfirmation(() => {
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

    public void RefreshList()
    {
        for(int i = 0; i<content.childCount; i++) Destroy(content.GetChild(i));
        items.Clear();

        string selectedSubjectId = DataManager.Instance.selectedSubjectId;

        Subject selectedSubject = DataManager.Instance.appData.subjects.Find(subject => subject.id == selectedSubjectId);

        if (addListItemPrefab)
        {
            Debug.Log(name+":Adding AddListItemPrefab into '"+content+"'");
            Button b = Instantiate(addListItemPrefab, content).GetComponentInChildren<Button>();
            b.onClick.AddListener(() => {OnAddTask();});
        }
        else
            Debug.LogError(name + ": No button for adding a list item configured.");

        // Show all tasks of a specific subject
        foreach (var task in DataManager.Instance.appData.tasks)
        {
            if (task.subjectId == selectedSubject.id)
                AddListItem(task.id, task.title);
        }
    }

    private void OnAddTask()
    {
        Debug.Log("Creating a new Task!");
        GetComponentInParent<Panel>().SwitchToPanel("config");
        configureTaskPanel.OpenForCreate();
    }

}