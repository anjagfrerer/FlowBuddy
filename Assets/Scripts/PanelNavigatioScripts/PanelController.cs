using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class PanelController : MonoBehaviour
{
    private Dictionary<string, Panel> panelLookup = new();

    void Awake()
    {
        var panels = GetComponentsInChildren<Panel>(true); // true = include inactive

        if(panels == null) Debug.LogWarning("No Panels detected!");

        foreach (var panel in panels)
        {
            AddPanel(panel);
        }
    }

    private void AddPanel(Panel panel)
    {
        if (!panelLookup.ContainsKey(panel.id)){
            panelLookup.Add(panel.id, panel);
            Debug.Log($"Added: {panel.id}");
        }
        else
            Debug.LogWarning($"Duplicate panel ID detected: '{panel.id}'!");
    }

    public void ShowPanel(string id)
    {
        if (!panelLookup.ContainsKey(id))
        {
            Debug.LogError($"PanelController: Unknown panel id '{id}'");
            return;
        }

        panelLookup[id].gameObject.SetActive(true);
    }

    public void ClosePanelByID(string id)
    {
        if (panelLookup.TryGetValue(id, out Panel panel))
        {
            panel.gameObject.SetActive(false);
        }
        else
            Debug.LogWarning($"PanelController: There is no Panel with id '{id}!'");
    }
}
