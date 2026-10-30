
[System.Serializable]
public class User
{
    public string username;
    public int energyLimit; // Löffel an einem normalen Tag (Spoon Theory)
    public int coins;

    // Tageswerte, gelten nur für energyDate (yyyy-MM-dd)
    public string energyDate;
    public int energySpentToday;
    public int todayEnergyMode = (int)DayEnergyMode.Normal;
}
