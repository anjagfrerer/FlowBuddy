using System;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// The Panel-Script is a Script for Identifieng Panels
/// They are attached to UI-Elements that should pop-up when a button is pressed
/// </summary>
public class Panel : MonoBehaviour
{
    public string Id;

    [NonSerialized]
    public PanelController panelController;

    /// <summary>
    /// Deactivates the Panel by Calling the panelController
    /// </summary>
    public void ClosePanel()
    {
        panelController.ShowPanel("");
        panelController.ClosePanelByID(Id);
    }

    public void SwitchToPanel(string id)
    {
        panelController.ShowPanel(id);
        panelController.ClosePanelByID(Id);
    }
}
