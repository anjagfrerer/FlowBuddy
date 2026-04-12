using UnityEngine;
using UnityEngine.InputSystem;

public class TapToChange : MonoBehaviour
{
    public SceneChanger sceneChanger;
    void Update()
    {
        // Pr�ft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            sceneChanger.loadNextScene();
        }

        // Mausklick f�r Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            sceneChanger.loadNextScene();
        }
    }
}
