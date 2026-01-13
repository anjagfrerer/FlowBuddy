using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger2 : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(3);
    }
}