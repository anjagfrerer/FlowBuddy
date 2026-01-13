using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject profilePanel;

    public Button backButton;
    public Button profileButton;

    private void Start()
    {
        backButton.onClick.AddListener(() => ShowPanel(homePanel));
        profileButton.onClick.AddListener(() => ShowPanel(profilePanel));

        ShowPanel(homePanel); // Standard Panel
    }

    private void ShowPanel(GameObject panelToShow)
    {
        // Alle Panels deaktivieren
        homePanel.SetActive(false);
        profilePanel.SetActive(false);

        // Gewähltes Panel aktivieren
        panelToShow.SetActive(true);
    }
}
