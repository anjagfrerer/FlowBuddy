using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TickableTaskItem : MonoBehaviour
{
    public TextMeshProUGUI subjectText;
    public TextMeshProUGUI titleText;
    public Toggle completeToggle;

    private Task currentTask;

    public void Setup(Task task)
    {
        currentTask = task;
        titleText.text = task.title;

        completeToggle.isOn = false;

        var subject = DataManager.Instance.appData.subjects.Find(s => s.id.ToString() == task.subjectId);
        subjectText.text = subject != null ? subject.name.Substring(0, 1).ToUpper() : "?";

        completeToggle.onValueChanged.RemoveAllListeners();
        completeToggle.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                // Wir starten den verzögerten Ablauf
                StartCoroutine(DelayedDeleteRoutine());
            }
        });
    }

    private IEnumerator DelayedDeleteRoutine()
    {
        // 1 Sekunde warten, damit der User den Haken auch sieht
        yield return new WaitForSeconds(1f);

        Debug.Log("Jetzt wäre der task theoretisch gelöscht worden.");

        // Hier rufen wir die eigentliche Lösch-Logik auf
        OnCompleteClicked();
    }

    private void OnCompleteClicked()
    {
        //currentTask.isDone = true;
        //DataManager.Instance.SaveData();

        // Das Objekt wird aus der Liste im UI entfernt
        // Destroy(gameObject);
    }
}