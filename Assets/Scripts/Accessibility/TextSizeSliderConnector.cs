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

    private void OnEnable()
    {
        StartCoroutine(LoadSavedValueRoutine());
    }

    private IEnumerator LoadSavedValueRoutine()
    {
        while (AccessibilityManager.Instance == null)
        {
            yield return null;
        }

        uiSlider.onValueChanged.RemoveAllListeners();
        uiSlider.value = AccessibilityManager.Instance.GetCurrentTextScale();
        uiSlider.onValueChanged.AddListener(AccessibilityManager.Instance.SetTextScale);
        
        isInitialized = true;
    }

    private void OnDisable()
    {
        if (uiSlider != null)
        {
            uiSlider.onValueChanged.RemoveAllListeners();
        }
        isInitialized = false;
    }
}