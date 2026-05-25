using UnityEngine;
using System;
using TMPro; 

public class AccessibilityManager : MonoBehaviour
{
    public static AccessibilityManager Instance { get; private set; }

    [Header("UI Komponenten")]
    [SerializeField] private GameObject tintOverlay;

    private bool isTintActive = false;
    
    // --- TEXTGRÖSSE ---
    public static event Action<float> OnTextScaleChanged;
    private float currentTextScale = 1.0f; 

    // --- SCHRIFTARTEN ---
    // Das Event übergibt jetzt das FontAsset UND den spezifischen Font-Größenfaktor!
    public static event Action<TMP_FontAsset, float> OnFontChanged;

    [Header("Verfügbare Schriftarten Assets")]
    public TMP_FontAsset standardFont;
    public TMP_FontAsset atkinsonFont;
    public TMP_FontAsset dyslexiaFont;

    [Header("Schriftarten Größen-Ausgleich (Multiplikator)")]
    [Tooltip("1.0 ist normal. Werte unter 1.0 machen die Schriftart kleiner.")]
    public float standardFontFactor = 1.0f;
    public float atkinsonFontFactor = 1.0f;
    public float dyslexiaFontFactor = 0.75f; // OpenDyslexic standardmäßig auf 75% runterskalieren

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ApplyTintState();
        
        OnTextScaleChanged?.Invoke(currentTextScale);

        // Beim Start direkt die Schriftart mit ihrem Faktor abfeuern
        TriggerFontUpdate();
    }

    public void SetTintActive(bool active)
    {
        isTintActive = active;
        PlayerPrefs.SetInt("Accessibility_TintActive", active ? 1 : 0);
        PlayerPrefs.Save();
        ApplyTintState();
    }

    public bool IsTintActive() => isTintActive;

    private void ApplyTintState()
    {
        if (tintOverlay != null) tintOverlay.SetActive(isTintActive);
    }

    // --- FUNKTIONEN FÜR TEXTGRÖSSE ---
    public void SetTextScale(float scaleValue)
    {
        currentTextScale = scaleValue;
        PlayerPrefs.SetFloat("Accessibility_TextScale", currentTextScale);
        PlayerPrefs.Save();
        OnTextScaleChanged?.Invoke(currentTextScale);
    }

    public float GetCurrentTextScale() => currentTextScale;


    // --- FUNKTIONEN FÜR SCHRIFTARTEN ---
    
    public TMP_FontAsset GetCurrentFont()
    {
        int fontIndex = PlayerPrefs.GetInt("Accessibility_FontIndex", 0);
        return GetFontFromIndex(fontIndex);
    }

    // Liefert den passenden Ausgleichsfaktor für die aktuelle Schriftart
    public float GetCurrentFontFactor()
    {
        int fontIndex = PlayerPrefs.GetInt("Accessibility_FontIndex", 0);
        switch (fontIndex)
        {
            case 1: return atkinsonFontFactor;
            case 2: return dyslexiaFontFactor;
            default: return standardFontFactor;
        }
    }

    public void ChangeFont(int fontIndex)
    {
        PlayerPrefs.SetInt("Accessibility_FontIndex", fontIndex);
        PlayerPrefs.Save();

        TriggerFontUpdate();
    }

    private void TriggerFontUpdate()
    {
        TMP_FontAsset newFont = GetCurrentFont();
        float currentFactor = GetCurrentFontFactor();
        
        // Event feuern und beide Werte an die Texte übergeben
        OnFontChanged?.Invoke(newFont, currentFactor);
    }

    private TMP_FontAsset GetFontFromIndex(int index)
    {
        switch (index)
        {
            case 1: return atkinsonFont;
            case 2: return dyslexiaFont;
            default: return standardFont; 
        }
    }

    private void LoadSettings()
    {
        isTintActive = PlayerPrefs.GetInt("Accessibility_TintActive", 0) == 1;
        currentTextScale = PlayerPrefs.GetFloat("Accessibility_TextScale", 1.0f);
    }
}