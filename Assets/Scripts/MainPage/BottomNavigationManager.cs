using UnityEngine;
using UnityEngine.UI;

public class BottomNavigationManager : MonoBehaviour
{
    public SceneChanger sceneChanger;
    public Button homeButton;
    public Button storeButton;
    public Button subjectsButton;

    private void Start()
    {
        homeButton.onClick.AddListener(() => sceneChanger.Load(SceneID.MainPage));
        storeButton.onClick.AddListener(() => sceneChanger.Load(SceneID.ShopPage));
        subjectsButton.onClick.AddListener(() => sceneChanger.Load(SceneID.SubjectPage));
    }
}
