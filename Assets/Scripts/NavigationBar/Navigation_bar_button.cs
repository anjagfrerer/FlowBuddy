
using UnityEngine;
using UnityEngine.UI;

public class Navigation_bar_button : MonoBehaviour
{
    [SerializeField] Image selectionOverlay;

    public Navigation_bar navigation_Bar;
    public SceneID scene_to_navigate = SceneID.MainPage;
    

    void Start()
    {
        if(SceneChanger.currentScene != scene_to_navigate)
            Select(false);
        else
            Select(true);

    }
    public void OnClick()
    {
        if(!selectionOverlay.isActiveAndEnabled)
        {
            SceneChanger.Load(scene_to_navigate); 
        }
    }

    void Select(bool select) => selectionOverlay.gameObject.SetActive(select);
}
