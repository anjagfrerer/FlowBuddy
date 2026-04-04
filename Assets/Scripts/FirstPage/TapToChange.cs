using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TapToChange : MonoBehaviour
{
    void Update()
    {
        // Pr�ft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            SceneManager.LoadScene(1);
        }

        // Mausklick f�r Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(1);
        }
    }
}
