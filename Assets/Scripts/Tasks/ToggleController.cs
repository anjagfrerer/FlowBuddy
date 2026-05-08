
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Image offImage;
    [SerializeField] private Image onImage;
    [SerializeField] private Toggle toggle;

    void Start()
    {
        UpdateImage();
    }
    public void UpdateImage()
    {
        offImage.gameObject.SetActive(!toggle.isOn);
        onImage.gameObject.SetActive(toggle.isOn);
    }

}