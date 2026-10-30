using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValueConnector : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text tMP_Text;
    [Tooltip("Zeigt beim Aufwands-Slider zusätzlich ein Wort, z.B. \"3 (Medium)\"")]
    [SerializeField] bool showEffortLabel = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        int value = (int)slider.value;
        tMP_Text.text = showEffortLabel ? $"{value} ({EnergyBudget.EffortLabel(value)})" : slider.value.ToString();
    }
}
