using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UI;

/// <summary>
/// The Panel Controller is the main controller for opening and closing Panels
/// This script should be attached to gameObjects,that store panels.
/// 
/// This script is used in the PanelContainer prefab
/// </summary>
public class PanelController : MonoBehaviour
{
    private Dictionary<string, Panel> panelLookup = new();

    [SerializeField]
    string InitPanelId = null;
    /*
    The component stores all Child-Panels in a Dictionary
    */
    void Awake()
    {
        var panels = GetComponentsInChildren<Panel>(true); // true = include inactive

        if (panels == null) Debug.LogWarning("No Panels detected!");

        foreach (var panel in panels)
        {
            AddPanel(panel);

            if (!string.IsNullOrEmpty(InitPanelId) && panel.Id != InitPanelId)
            {
                panel.gameObject.SetActive(false);
            }
            else
            {
                panel.gameObject.SetActive(true);
            }
        }
    }

    /*
    Before adding a panel, it checks if the id already exists
    */
    private void AddPanel(Panel panel)
    {
        panel.panelController = this;
        if (!panelLookup.ContainsKey(panel.Id))
        {
            panelLookup.Add(panel.Id, panel);
            Debug.Log($"Added: {panel.Id}");
        }
        else
            Debug.LogError($"{name}: Duplicate panel ID detected: '{panel.Id}'!");
    }

    /// <summary>
    /// Use this function whenever you want to display a panel on input.
    /// 
    /// the UI-Element must have the 'Panel' Script attached and
    /// the id must match with the Panel-Id of that UI-Element.
    /// 
    /// The Panel-Id is part of the 'Panel' Script and can be set in the inspector
    /// </summary>
    /// <param name="id"></param>
    public void ShowPanel(string id)
    {
        if(string.IsNullOrEmpty(id)){
            panelLookup[InitPanelId].gameObject.SetActive(true);
            return;    
        }

        if (!panelLookup.ContainsKey(id))
        {
            Debug.LogError($"PanelController: Unknown panel id '{id}'");
            return;
        }

        panelLookup[id].gameObject.SetActive(true);
    }

    /// <summary>
    /// Use this function whenever you want to hide a panel on input.
    /// 
    /// the UI-Element must have the 'Panel' Script attached and
    /// the id must match with the Panel-Id of that UI-Element.
    /// 
    /// The Panel-Id is part of the 'Panel' Script and can be set in the inspector
    /// 
    /// ! for close-buttons inside the gameobject, it is advised to use the 'ClosePanelScript' Script
    /// </summary>
    /// <param name="id"></param>
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
