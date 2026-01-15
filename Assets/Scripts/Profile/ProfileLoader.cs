using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileLoader : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject profilePrefab;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject homePanel;

    private string[] profiles = new string[]
    {
        "Personal Data",
        "Color Scheme / Theme",
        "Adjust Text Size",
        "Choose Font",
        "Colorblind / Contrast Modes",
        "Sound Settings"
    };

    // ⚡ Öffentliche Methode, die beim Klick aufgerufen wird
    public void LoadProfiles()
    {
        if (content == null || profilePrefab == null)
        {
            Debug.LogError("Content oder ProfilePrefab nicht gesetzt!");
            return;
        }

        ClearContent();

        foreach (string subject in profiles)
        {
            CreateButton(subject);
        }

        // Panel wechseln
        if (homePanel != null) homePanel.SetActive(false);
        if (profilePanel != null) profilePanel.SetActive(true);
    }

    private void CreateButton(string profile)
    {
        GameObject buttonObj = Instantiate(profilePrefab, content);

        TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = profile;

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            string capturedProfile = profile;
            button.onClick.AddListener(() =>
            {
                Debug.Log("Clicked: " + capturedProfile);
            });
        }
    }

    private void ClearContent()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);
    }
}
