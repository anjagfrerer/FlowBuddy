using UnityEngine;

public class ClosePanelScript : MonoBehaviour
{
    [SerializeField] private PanelController controller;
    public void CloseOwnPanel()
    {
        controller.ClosePanelByID(GetComponentInParent<Panel>().id);
    }
    public void ClosePanelByID(string id)
    {
        controller.ClosePanelByID(id);
    }
}

