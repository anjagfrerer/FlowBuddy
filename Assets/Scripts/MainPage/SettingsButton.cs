using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [Header("Panels")]
    public GameObject homePanel;
    public GameObject profilePanel;

    // Zeigt ein Panel und versteckt die anderen
    public void ShowProfilePanel()
    {
        homePanel.SetActive(false);
        profilePanel.SetActive(true);
    }

    public void ShowHomePanel()
    {
        homePanel.SetActive(true);
        profilePanel.SetActive(false);
    }
}
