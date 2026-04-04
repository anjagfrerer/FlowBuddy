using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static SceneDatabase Database;
    public static void Load(SceneID id)
    {
        string sceneName = Database.GetSceneName(id);
        SceneManager.LoadScene(sceneName);
    }
}