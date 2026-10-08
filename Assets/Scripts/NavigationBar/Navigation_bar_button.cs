
using UnityEngine;
using UnityEngine.UI;

public class Navigation_bar_button : MonoBehaviour
{
    [SerializeField] Image selectionOverlay;

    public Navigation_bar navigation_Bar;
    public SceneID scene_to_navigate = SceneID.MainPage;
    

    void Start()
    {
        selectionOverlay.gameObject.SetActive(false);
    }
    public void OnClick()
    {
        if(!selectionOverlay.isActiveAndEnabled)
        {
            print(name + ": Clicked");
            navigation_Bar.Select(this);  
        }
    }

    public void Select(bool select) => selectionOverlay.gameObject.SetActive(select);
}
