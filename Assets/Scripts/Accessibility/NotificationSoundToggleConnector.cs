using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class NotificationSoundToggleConnector : MonoBehaviour
{
    private Toggle uiToggle;

    private void Awake()
    {
        uiToggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        if (NotificationSettingsManager.Instance != null && uiToggle != null)
        {
            uiToggle.isOn = NotificationSettingsManager.Instance.IsSoundEnabled();
            uiToggle.onValueChanged.AddListener(NotificationSettingsManager.Instance.SetSoundEnabled);
            
            // Deaktiviert die Ton-Checkbox, falls Benachrichtigungen generell aus sind
            uiToggle.interactable = NotificationSettingsManager.Instance.AreNotificationsEnabled();
        }
    }
}