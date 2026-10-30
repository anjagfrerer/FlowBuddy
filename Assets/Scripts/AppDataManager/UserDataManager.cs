using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    // Ändert den bestehenden User, statt ihn zu ersetzen (sonst gehen Münzen und Energie verloren)
    public void setUserName(string name)
    {
        DataManager.Instance.appData.user.username = name;
        DataManager.Instance.SaveData();
    }

    public string GetUserName() => DataManager.Instance.appData.user.username;

    public void SetEnergyLevel(int level) => EnergyBudget.SetBaseline(level);

    public int GetEnergyLevel() => EnergyBudget.Baseline;
}
