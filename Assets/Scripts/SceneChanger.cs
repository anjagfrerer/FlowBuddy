using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static SceneDatabase Database;
    public static SceneID currentScene;


    // This attribute tells Unity to run this method automatically on game start
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeDatabase()
    {
        // Automatically loads the asset from the Resources folder
        Database = Resources.Load<SceneDatabase>("SceneDatabase");

        if (Database == null)
        {
            Debug.LogError("SceneChanger failed to initialize! 'SceneDatabase' asset not found in Resources folder.");
        }
    }
    public static void Load(SceneID id)
    {
        if(Database == null)
            return;
        currentScene = id;
        string sceneName = Database.GetSceneName(id);
        SceneManager.LoadScene(sceneName);
    }
}