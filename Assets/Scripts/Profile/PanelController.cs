using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
struct PanelEntry
{    
    public string id;       
    public GameObject panel; 
}

public class PanelController : MonoBehaviour
{
    [Header("UI References")]
    
    [SerializeField] private List<PanelEntry> panels;
    private Dictionary<string, GameObject> panelLookup;

    private string activePanel;

    void Awake()
    {
        panelLookup = new Dictionary<string, GameObject>();
        activePanel = null;

        foreach(var entry in panels)
        {
            if(!panelLookup.ContainsKey(entry.id))
                panelLookup.Add(entry.id, entry.panel);
            entry.panel.SetActive(false);
        }
    }

    public void ShowPanel(string id)
    {
        // deactivate
        foreach(var entry in panels)
            entry.panel.SetActive(false);

        // activate target

        if (!panelLookup.ContainsKey(id))
        {
            Debug.LogError($"PanelController: Unknown panel id '{id}'");
        }

        panelLookup[id].SetActive(true);
        activePanel = id;
    }

    public void ClosePanel()
    {
        if(activePanel == null)
        {
            Debug.LogWarning("PanelController: The currently active panel is 'null'");
            return;
        }
        if (panelLookup[activePanel].activeSelf)
        {
            panelLookup[activePanel].SetActive(false);
            activePanel = null;
        }
    }
}
