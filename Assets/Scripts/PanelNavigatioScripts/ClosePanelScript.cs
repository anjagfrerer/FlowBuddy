using UnityEngine;

/// <summary>
/// The ClosePanel - Script is a script for closing panels
/// 
/// It should be attached to buttons that should close panels.
/// When closing, the UI-Elements get deactivated.
/// </summary>
public class ClosePanelScript : MonoBehaviour
{
    [SerializeField] private PanelController controller;

    /// <summary>
    /// This function is used to close Panels with an internal Button
    /// That means, that the Bu
    /// </summary>
    public void CloseOwnPanel()
    {
        controller.ClosePanelByID(GetComponentInParent<Panel>().id);
    }

    /// <summary>
    /// Used for External Buttons, i.e buttons that aren't direct child of the UI-Element
    /// </summary>
    /// <param name="id">...Panel id</param>
    public void ClosePanelByID(string id)
    {
        controller.ClosePanelByID(id);
    }
}

