using UnityEngine;
using UnityEngine.UI;

public class BottomNavigationManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject storePanel;
    public GameObject subjectsPanel;

    public Button homeButton;
    public Button storeButton;
    public Button subjectsButton;

    private void Start()
    {
        homeButton.onClick.AddListener(() => ShowPanel(homePanel));
        storeButton.onClick.AddListener(() => ShowPanel(storePanel));
        subjectsButton.onClick.AddListener(() => ShowPanel(subjectsPanel));

        ShowPanel(homePanel); // Standard Panel
    }

    private void ShowPanel(GameObject panelToShow)
    {
        // Alle Panels deaktivieren
        homePanel.SetActive(false);
        storePanel.SetActive(false);
        subjectsPanel.SetActive(false);

        // Gewähltes Panel aktivieren
        panelToShow.SetActive(true);
    }
}
