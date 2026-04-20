using UnityEngine;
using UnityEngine.InputSystem;

public class TitlePageTapToChange : MonoBehaviour
{
    public SceneChanger sceneChanger;

    void Start()
    {
        if(DataManager.Instance.appData.user.username != null)
            sceneChanger.nextScene = SceneID.MainPage;
        else
            sceneChanger.nextScene = SceneID.FirstStart;
        
    }
    void Update()
    {
        // Prüft, ob irgendein Touch auf dem Bildschirm ist
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            sceneChanger.loadNextScene();
        }

        // Mausklick für Editor/PC
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            sceneChanger.loadNextScene();
        }
    }
}
