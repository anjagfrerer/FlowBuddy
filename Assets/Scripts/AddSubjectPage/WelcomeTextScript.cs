using TMPro;
using UnityEngine;

public class WelcomeTextScript : MonoBehaviour
{
    void Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        string username = DataManager.Instance.appData.user.username;

        if(text == null)
        {
            Debug.LogError("Component TMP_Text not found!");
            return;
        }

        if(username != null)
            text.text = $"Hello, {username}!";
        else
        {
            text.text = $"Hello, new User !";
            Debug.LogWarning("User is undefined!");
        }
    }

}
