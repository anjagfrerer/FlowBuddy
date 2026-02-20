using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TapToChange : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // Prüft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            SceneManager.LoadScene(1);
        }

        // Mausklick für Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(1);
        }
    }
}
