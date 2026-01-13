using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListView : MonoBehaviour
{
    public Transform content;
    public GameObject listItemPrefab;
    public TMP_InputField inputField;
    public Button addButton;

    private List<GameObject> items = new List<GameObject>();

    private void Start()
    {
        addButton.onClick.AddListener(OnAddButtonClicked);
        RefreshList();
    }

    private void OnAddButtonClicked()
    {
        string name = inputField.text.Trim();
        if (string.IsNullOrEmpty(name)) return;

        SubjectController.Instance.AddSubject(name);
        AddListItem(name);
        inputField.text = "";
    }

    public void RefreshList()
    {
        // Alte Items löschen
        foreach (var item in items) Destroy(item);
        items.Clear();

        // Alle Subjects aus Controller holen
        List<string> subjects = SubjectController.Instance.GetAllSubjects();
        foreach (var s in subjects)
        {
            AddListItem(s);
        }
    }

    private void AddListItem(string name)
    {
        GameObject newItem = Instantiate(listItemPrefab, content);
        newItem.transform.Find("Text").GetComponent<TMP_Text>().text = name;

        Button deleteBtn = newItem.transform.Find("DeleteButton").GetComponent<Button>();
        deleteBtn.onClick.AddListener(() =>
        {
            SubjectController.Instance.DeleteSubject(name);
            items.Remove(newItem);
            Destroy(newItem);
        });

        items.Add(newItem);
    }
}
