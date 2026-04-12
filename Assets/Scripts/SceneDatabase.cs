using UnityEngine;

[CreateAssetMenu(menuName = "Scene Management/Scene Database")]
public class SceneDatabase : ScriptableObject
{
    [System.Serializable]
    public struct SceneEntry
    {
        public SceneID id;
        public string sceneName;
    }

    public SceneEntry[] scenes;

    public string GetSceneName(SceneID id)
    {
        foreach (var entry in scenes)
            if (entry.id == id)
                return entry.sceneName;

        Debug.LogError($"Scene ID {id} not found in SceneDatabase!");
        return null;
    }
}
