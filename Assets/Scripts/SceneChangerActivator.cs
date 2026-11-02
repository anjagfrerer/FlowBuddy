using UnityEngine;

public class SceneChangerActivator : MonoBehaviour
{
    [SerializeField]
    SceneID nextScene = SceneID.MainPage;
    public void Load()
    {
        Debug.Log(name+": Loading Scene " + nextScene);
        SceneChanger.Load(nextScene);
    }
}
