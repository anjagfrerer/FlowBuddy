using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class NotificationToggleConnector : MonoBehaviour
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
            uiToggle.isOn = NotificationSettingsManager.Instance.AreNotificationsEnabled();
            uiToggle.onValueChanged.AddListener(NotificationSettingsManager.Instance.SetNotificationsEnabled);
        }
    }
}