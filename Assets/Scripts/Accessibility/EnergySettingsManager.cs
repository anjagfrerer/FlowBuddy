using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySettingsManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnergyPreset
    {
        public string label;
        public Button stageButton;
    }

    [Header("UI Configuration")]
    [Tooltip("Reihenfolge wie EnergyBudget.BaselinePresets: Low, Medium, High, Very High")]
    [SerializeField] private EnergyPreset[] energyPresets = new EnergyPreset[4];

    [Header("Visual Feedback colors")]
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color deselectedColor = Color.white;

    private void Start()
    {
        InitializePresets();

        int savedIndex = System.Array.IndexOf(EnergyBudget.BaselinePresets, EnergyBudget.ClosestPreset(EnergyBudget.Baseline));
        Debug.Log($"[EnergySettings] Basiswert geladen: {EnergyBudget.Baseline} Löffel.");
        UpdateUIAndVisuals(savedIndex);
    }

    private void InitializePresets()
    {
        string[] defaultLabels = { "Low", "Medium", "High", "Very High" };

        for (int i = 0; i < energyPresets.Length; i++)
        {
            int index = i;
            if (string.IsNullOrEmpty(energyPresets[index].label) && index < defaultLabels.Length)
                energyPresets[index].label = defaultLabels[index];

            if (energyPresets[index].stageButton != null)
            {
                energyPresets[index].stageButton.onClick.AddListener(() => OnPresetSelected(index));
            }
        }
    }

    private void OnPresetSelected(int selectedIndex)
    {
        if (selectedIndex >= EnergyBudget.BaselinePresets.Length) return;

        EnergyBudget.SetBaseline(EnergyBudget.BaselinePresets[selectedIndex]);
        Debug.Log($"[EnergySettings] {energyPresets[selectedIndex].label}: {EnergyBudget.Baseline} Löffel gespeichert.");
        UpdateUIAndVisuals(selectedIndex);
    }

    private void UpdateUIAndVisuals(int activeIndex)
    {
        for (int i = 0; i < energyPresets.Length; i++)
        {
            Button button = energyPresets[i].stageButton;
            if (button == null) continue;

            bool isActive = i == activeIndex;
            ColorBlock cb = button.colors;
            cb.normalColor = isActive ? selectedColor : deselectedColor;
            cb.selectedColor = isActive ? selectedColor : deselectedColor;
            button.colors = cb;

            // Auswahl nicht nur über Farbe zeigen (Farbenblindheit)
            TMP_Text label = button.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.fontStyle = isActive ? FontStyles.Bold | FontStyles.Underline : FontStyles.Normal;
        }
    }
}
