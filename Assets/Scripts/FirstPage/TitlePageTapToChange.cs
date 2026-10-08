using UnityEngine;
using UnityEngine.InputSystem;

public class TitlePageTapToChange : MonoBehaviour
{
    [SerializeField]
    SceneID nextScene;

    void Start()
    {
        if(DataManager.Instance.appData.user.username != null)
            SceneChanger.Load(SceneID.MainPage);
        else
            SceneChanger.Load(SceneID.FirstStart);
    }
    void Update()
    {
        // Prüft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            SceneChanger.Load(nextScene);
        }

        // Mausklick für Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneChanger.Load(nextScene);
        }
    }
}
