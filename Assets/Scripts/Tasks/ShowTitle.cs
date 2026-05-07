using TMPro;
using UnityEngine;

public class ShowTitle : MonoBehaviour
{
    public TMP_Text subjectTitle;

    void Start()
    {
        

        if (DataManager.Instance.selectedSubjectName == "")
        {
            Debug.LogWarning("Subject not found");
            subjectTitle.text = "Tasks:";
        } else
        {
            subjectTitle.text = DataManager.Instance.selectedSubjectName + " tasks:";
        }
    }
}
