using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubjectListLoader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject subjectPrefab;

    private void OnEnable()
{

    Invoke(nameof(DelayedLoad), 0.02f);
}

private void DelayedLoad()
{
    LoadSubjects();
}

    private void LoadSubjects()
    {
        ClearContent();

        if (DataManager.Instance == null ||
            DataManager.Instance.appData == null ||
            DataManager.Instance.appData.subjects == null)
        {
            Debug.LogWarning("No subjects found");
            return;
        }

        foreach (var subject in DataManager.Instance.appData.subjects)
        {
            CreateButton(subject.name, subject.id);
        }
    }

    private void CreateButton(string subjectName, string subjectId)
    {
        GameObject buttonObj = Instantiate(subjectPrefab, content);

        TMP_Text text = buttonObj.transform.Find("Text").GetComponent<TMP_Text>();
        text.text = subjectName;

        Button button = buttonObj.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Debug.Log("Subject clicked: " + subjectName);
            DataManager.Instance.selectedSubjectName = subjectName; // DataManager melden, welches Fach gewählt wurde
            DataManager.Instance.selectedSubjectId = subjectId;
            Debug.Log("Subject selected");


            SceneChanger.Load(SceneID.TaskPage);

        });
    }

    private void ClearContent()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
