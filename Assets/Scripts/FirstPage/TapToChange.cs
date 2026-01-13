using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TapToChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Prüft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            SceneManager.LoadScene(1);
        }

        // Optional: Mausklick für Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(1);
        }
    }
}
