using UnityEngine;
using UnityEngine.UI;
using System;

public class EnergySettingsManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnergyPreset
    {
        public string label;
        public int minutes;
        public Button stageButton;
    }

    [Header("UI Configuration")]
    [SerializeField] private EnergyPreset[] energyPresets = new EnergyPreset[4];
    
    [Header("Visual Feedback colors")]
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color deselectedColor = Color.white;

    private const string EnergyPrefsKey = "User_Selected_EnergyLimit";
    private int currentSavedLimit = 60; 

    private void Start()
    {
        InitializePresets();
        LoadLimitFromPrefs();
    }

    private void InitializePresets()
    {
        if (energyPresets[0].minutes == 0)
        {
            energyPresets[0].label = "Low"; energyPresets[0].minutes = 30;
            energyPresets[1].label = "Medium"; energyPresets[1].minutes = 60;
            energyPresets[2].label = "High"; energyPresets[2].minutes = 90;
            energyPresets[3].label = "Very High"; energyPresets[3].minutes = 120;
        }

        for (int i = 0; i < energyPresets.Length; i++)
        {
            int index = i;
            if (energyPresets[index].stageButton != null)
            {
                energyPresets[index].stageButton.onClick.AddListener(() => OnPresetSelected(index));
            }
        }
    }

    private void LoadLimitFromPrefs()
    {
        currentSavedLimit = PlayerPrefs.GetInt(EnergyPrefsKey, 60);
        Debug.Log($"[EnergySettings] Limit aus PlayerPrefs geladen: {currentSavedLimit} Min.");
        
        UpdateUIAndVisuals(GetClosestPresetIndex(currentSavedLimit));
    }

    private void OnPresetSelected(int selectedIndex)
    {
        currentSavedLimit = energyPresets[selectedIndex].minutes;

        PlayerPrefs.SetInt(EnergyPrefsKey, currentSavedLimit);
        PlayerPrefs.Save();
        
        Debug.Log($"[EnergySettings] {currentSavedLimit} Min erfolgreich in PlayerPrefs gesichert!");
        UpdateUIAndVisuals(selectedIndex);
    }

    private int GetClosestPresetIndex(int minutes)
    {
        int closestIndex = 1;
        int smallestDifference = int.MaxValue;

        for (int i = 0; i < energyPresets.Length; i++)
        {
            int diff = Mathf.Abs(energyPresets[i].minutes - minutes);
            if (diff < smallestDifference)
            {
                smallestDifference = diff;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    private void UpdateUIAndVisuals(int activeIndex)
    {
        for (int i = 0; i < energyPresets.Length; i++)
        {
            if (energyPresets[i].stageButton != null)
            {
                ColorBlock cb = energyPresets[i].stageButton.colors;
                cb.normalColor = (i == activeIndex) ? selectedColor : deselectedColor;
                cb.selectedColor = (i == activeIndex) ? selectedColor : deselectedColor;
                energyPresets[i].stageButton.colors = cb;
            }
        }
    }
}