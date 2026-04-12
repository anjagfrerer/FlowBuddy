using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public SceneID nextScene;
    public SceneDatabase Database;
    public void Load(SceneID id)
    {
        string sceneName = Database.GetSceneName(id);
        SceneManager.LoadScene(sceneName);
    }

    public void loadNextScene()
    {
        string sceneName = Database.GetSceneName(nextScene);
        SceneManager.LoadScene(sceneName);
    }
}