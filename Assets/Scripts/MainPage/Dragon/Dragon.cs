using UnityEngine;

/// <summary>
/// The Dragon Script makes the dragon an instance, and therefore persists his gameobject through scenes
/// </summary>
public class Dragon : MonoBehaviour
{
    static Dragon Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    
}