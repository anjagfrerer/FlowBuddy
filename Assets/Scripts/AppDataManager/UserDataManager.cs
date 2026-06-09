using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    private User user = new User();

    public void setUserName(string name)
    {
        user.username = name;
        UpdateAppdata();
    }

    public string GetUserName() => DataManager.Instance.appData.user.username;

    public void SetEnergyLevel(int level)
    {
        user.energyLimit = level;
        UpdateAppdata();
    }

    public int GetEnergyLevel() => DataManager.Instance.appData.user.energyLimit;

    void UpdateAppdata()
    {
        DataManager.Instance.appData.user = user;
        DataManager.Instance.SaveData();
    }


}