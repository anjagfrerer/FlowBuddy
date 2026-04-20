using TMPro;
using UnityEngine;

public class UserTextScript : MonoBehaviour
{
    public string contentText;
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
            text.text = contentText.Replace("{User}",username);
        else
        {
            gameObject.SetActive(false);
            Debug.LogWarning("User is undefined!");
        }
    }

}
