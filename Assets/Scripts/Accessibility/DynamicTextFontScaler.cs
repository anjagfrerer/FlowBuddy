using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DynamicTextFontScaler : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    
    [Header("Einstellungen")]
    [Tooltip("Trage hier die normale Schriftgröße ein, die der Text standardmäßig haben soll.")]
    [SerializeField] private float originalFontSize = 48f; 

    // Feste Textgröße statt Slider: Fließtext (48) wird 42 Einheiten. Bei 1080x1920-Referenz
    // sind das ca. 16 sp auf heutigen Handys (ab 360x800 dp), auf alten 16:9-Geräten (360x640 dp) noch 14 sp.
    // 16 sp = Material "Body Large", ca. 12 pt der BDA-Empfehlung (12-14 pt) für Dyslexie.
    // Titel und Hinweise schrumpfen im selben Verhältnis, die Größen-Hierarchie bleibt.
    private const float FixedTextScale = 42f / 48f;

    // Auto Size darf nur leicht verkleinern, wenn der Text sonst nicht passt: 37/42 = ca. 14 sp (Material "Body Medium")
    private const float MinAutoSizeFactor = 37f / 42f;

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

        // Feste Größe auch ohne AccessibilityManager (z. B. Szene direkt im Editor gestartet)
        UpdateFinalFontSize();
    }

    private void OnDisable()
    {
        AccessibilityManager.OnTextScaleChanged -= AdjustFontSize;
        AccessibilityManager.OnFontChanged -= ApplyFont;
    }

    // Wenn der Slider bewegt wird: Wert wird nur gemerkt, die Größe bleibt fest (FixedTextScale)
    private void AdjustFontSize(float scaleFactor)
    {
        currentScaleFactor = scaleFactor;
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
            // Formel: Basis-Größe * feste Skalierung * Font-Ausgleich (z.B. 0.75), der Slider-Wert zählt nicht mehr
            float size = originalFontSize * FixedTextScale * currentFontSpecificFactor;
            textComponent.fontSize = size;

            // Sonst wählt Auto Size je nach Box eine beliebige Größe und die Texte sind uneinheitlich
            if (textComponent.enableAutoSizing)
            {
                textComponent.fontSizeMax = size;
                textComponent.fontSizeMin = size * MinAutoSizeFactor;
            }
            textComponent.ForceMeshUpdate(); 
        }
    }
}