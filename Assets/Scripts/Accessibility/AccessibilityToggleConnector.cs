using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AccessibilityToggleConnector : MonoBehaviour
{
    private Toggle uiToggle;

    private void Awake()
    {
        uiToggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        if (AccessibilityManager.Instance != null && uiToggle != null)
        {
            uiToggle.isOn = AccessibilityManager.Instance.IsTintActive();
            uiToggle.onValueChanged.AddListener(AccessibilityManager.Instance.SetTintActive);
        }
        else
        {
            Debug.LogWarning("AccessibilityManager not found. Please start the game from the main scene!");
        }
    }
}