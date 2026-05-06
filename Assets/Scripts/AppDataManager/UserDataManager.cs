using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    private User user = new User();

    public void setUserName(string name)
    {
        user.username = name;
        UpdateAppdata();
    }
    void UpdateAppdata()
    {
        DataManager.Instance.appData.user = user;
        DataManager.Instance.SaveData();
    }

}