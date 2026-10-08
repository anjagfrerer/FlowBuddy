using UnityEngine;

public class SceneChangerActivator : MonoBehaviour
{
    [SerializeField]
    SceneID next_scene;
    public void Load()
    {
        SceneChanger.Load(next_scene);
    }
}
