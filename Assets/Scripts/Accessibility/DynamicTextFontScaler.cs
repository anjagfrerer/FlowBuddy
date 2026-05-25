using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DynamicTextFontScaler : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    
    [Header("Einstellungen")]
    [Tooltip("Trage hier die normale Schriftgröße ein, die der Text standardmäßig haben soll.")]
    [SerializeField] private float originalFontSize = 48f; 

    private float currentScaleFactor = 1.0f;
    private float currentFontSpecificFactor = 1.0f;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        AccessibilityManager.OnTextScaleChanged += AdjustFontSize;
        AccessibilityManager.OnFontChanged += ApplyFont;
        
        if (AccessibilityManager.Instance != null)
        {
            // Werte direkt beim Laden der Seite initialisieren
            currentScaleFactor = AccessibilityManager.Instance.GetCurrentTextScale();
            currentFontSpecificFactor = AccessibilityManager.Instance.GetCurrentFontFactor();
            
            ApplyFont(AccessibilityManager.Instance.GetCurrentFont(), currentFontSpecificFactor);
        }
    }

    private void OnDisable()
    {
        AccessibilityManager.OnTextScaleChanged -= AdjustFontSize;
        AccessibilityManager.OnFontChanged -= ApplyFont;
    }

    // Wenn der Slider bewegt wird
    private void AdjustFontSize(float scaleFactor)
    {
        currentScaleFactor = scaleFactor;
        UpdateFinalFontSize();
    }

    // Wenn die Schriftart im Dropdown gewechselt wird
    private void ApplyFont(TMP_FontAsset newFont, float fontSpecificFactor)
    {
        if (textComponent != null && newFont != null)
        {
            textComponent.font = newFont;
            currentFontSpecificFactor = fontSpecificFactor;
            
            UpdateFinalFontSize();
        }
    }

    // Berechnet die finale Größe aus allen Faktoren zusammen
    private void UpdateFinalFontSize()
    {
        if (textComponent != null)
        {
            // Formel: Basis-Größe * Slider (z.B. 1.2) * Font-Ausgleich (z.B. 0.75)
            textComponent.fontSize = originalFontSize * currentScaleFactor * currentFontSpecificFactor;
            textComponent.ForceMeshUpdate(); 
        }
    }
}