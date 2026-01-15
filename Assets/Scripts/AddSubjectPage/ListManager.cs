using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public Button addButton;
    public Transform content;
    public GameObject listItemPrefab;

    private List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        addButton.onClick.AddListener(OnAddButtonClicked);
        RefreshList();
    }

    private void OnAddButtonClicked()
    {
        string subjectName = inputField.text.Trim();
        if (string.IsNullOrEmpty(subjectName))
            return;

        DataManager.Instance.AddSubject(subjectName);
        AddListItem(subjectName);
        inputField.text = "";
    }

    private void AddListItem(string subjectName)
    {
        GameObject newItem = Instantiate(listItemPrefab, content);
        newItem.transform.Find("Text").GetComponent<TMP_Text>().text = subjectName;

        Button deleteButton = newItem.transform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => OnDeleteButtonClicked(newItem, subjectName));

        items.Add(newItem);
    }

    private void OnDeleteButtonClicked(GameObject item, string subjectName)
    {
        DataManager.Instance.DeleteSubject(subjectName);
        items.Remove(item);
        Destroy(item);
    }

    private void RefreshList()
    {
        foreach (var subject in DataManager.Instance.appData.subjects)
        {
            AddListItem(subject.name);
        }
    }
}
