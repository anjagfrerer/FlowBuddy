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
        // Wir suchen den globalen Manager, der im Hauptmenü gestartet wurde
        if (AccessibilityManager.Instance != null && uiToggle != null)
        {
            // Schalter auf den gespeicherten Zustand setzen (Haken rein oder raus)
            uiToggle.isOn = AccessibilityManager.Instance.IsTintActive();
            
            // Dem Schalter beibringen, was er tun soll, wenn man ihn anklickt
            uiToggle.onValueChanged.AddListener(AccessibilityManager.Instance.SetTintActive);
        }
        else
        {
            // Falls man die Szene einzeln im Editor startet, existiert der Manager nicht
            Debug.LogWarning("AccessibilityManager nicht gefunden. Starte das Spiel über die Start-Szene!");
        }
    }
}