using UnityEngine;
using UnityEngine.UI; 

[RequireComponent(typeof(Slider))]
public class SatisfactionSliderUI : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        DragonSatisfactionManager.OnSatisfactionChanged += UpdateSliderVisuals;

        if (DragonSatisfactionManager.Instance != null)
        {
            UpdateSliderVisuals(DragonSatisfactionManager.Instance.GetCurrentSatisfaction());
        }
    }

    private void OnDisable()
    {
        DragonSatisfactionManager.OnSatisfactionChanged -= UpdateSliderVisuals;
    }

    private void UpdateSliderVisuals(float percentage)
    {
        if (slider != null)
        {
            slider.value = percentage;
        }
    }
}