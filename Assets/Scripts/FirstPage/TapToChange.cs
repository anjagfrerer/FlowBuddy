using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TapToChange : MonoBehaviour
{
    public SceneDatabase sceneDatabase;
    public SceneID nextScene;

    void Awake()
    {
        SceneChanger.Database = sceneDatabase;
    }
    void Update()
    {
        // Pr�ft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            SceneChanger.Load(nextScene);
        }

        // Mausklick f�r Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneChanger.Load(nextScene);
        }
    }
}
