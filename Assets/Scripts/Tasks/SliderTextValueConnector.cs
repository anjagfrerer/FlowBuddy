using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValueConnector : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Text tMP_Text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       tMP_Text.text = slider.value.ToString(); 
    }

    public void UpdateText()
    {
        tMP_Text.text = slider.value.ToString(); 
    }
}
