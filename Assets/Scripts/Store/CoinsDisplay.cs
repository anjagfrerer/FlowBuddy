using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
    public TMP_Text coinsDisplay;

    public void Start()
    {
        DisplayCoins();
    }

    public void DisplayCoins()
    {
        coinsDisplay.text = DataManager.Instance.appData.user.coins.ToString();
    }
}
