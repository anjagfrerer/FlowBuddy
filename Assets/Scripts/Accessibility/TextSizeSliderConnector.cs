using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class TextSizeSliderConnector : MonoBehaviour
{
    private Slider uiSlider;
    private bool isInitialized = false;

    private void Awake()
    {
        uiSlider = GetComponent<Slider>();
    }

    // Wird JEDES MAL aufgerufen, wenn das Settings-Panel per SetActive(true) sichtbar gemacht wird
    private void OnEnable()
    {
        // Wir starten eine robuste Routine, die wartet, bis der Manager wirklich da ist
        StartCoroutine(LoadSavedValueRoutine());
    }

    private IEnumerator LoadSavedValueRoutine()
    {
        // Sicherungs-Schleife: Wir warten, bis der globale Manager existiert.
        // Das verhindert Fehler, selbst wenn die Szene gerade erst lädt.
        while (AccessibilityManager.Instance == null)
        {
            yield return null; // Warte einen Frame und versuche es erneut
        }

        // Alten Listener entfernen, damit beim mehrmaligen Öffnen nichts doppelt registriert wird
        uiSlider.onValueChanged.RemoveAllListeners();

        // Wert aus dem Manager auslesen und auf den Slider übertragen
        uiSlider.value = AccessibilityManager.Instance.GetCurrentTextScale();

        // Den Slider wieder scharf schalten, sodass er Änderungen ans System funkt
        uiSlider.onValueChanged.AddListener(AccessibilityManager.Instance.SetTextScale);
        
        isInitialized = true;
    }

    private void OnDisable()
    {
        if (uiSlider != null)
        {
            // Listener entfernen, wenn das Panel wieder deaktiviert wird
            uiSlider.onValueChanged.RemoveAllListeners();
        }
        isInitialized = false;
    }
}