using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class FontDropdownConnector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        // Dropdown Optionen im Code säubern und neu befüllen
        dropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string> { "Standard", "Atkinson Legible", "OpenDyslexic" };
        dropdown.AddOptions(options);

        // Den aktuell gespeicherten Stand einstellen
        int savedIndex = PlayerPrefs.GetInt("Accessibility_FontIndex", 0);
        dropdown.value = savedIndex;

        // Auf Änderungen reagieren
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int index)
    {
        if (AccessibilityManager.Instance != null)
        {
            AccessibilityManager.Instance.ChangeFont(index);
        }
    }
}