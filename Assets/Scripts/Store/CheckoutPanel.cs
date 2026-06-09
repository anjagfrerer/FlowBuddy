using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutPanel : MonoBehaviour
{
    public TMP_Text itemInfoText;
    public Button buyButton;

    private ShopItem selectedItem;

    public void SelectItem(ShopItem item)
    {
        selectedItem = item;
        itemInfoText.text = item.name + ": " + item.price + " coins";
    }

    public void OnBuyClicked()
    {
        Debug.Log("Coins: " + DataManager.Instance.appData.user.coins + " Price: " + selectedItem.price);

        if (selectedItem == null)
        {
            Debug.Log("Please select an item first!");
            return;
        }

        if (DataManager.Instance.appData.user.coins >= selectedItem.price)
        {
            DataManager.Instance.appData.user.coins -= selectedItem.price;
            DataManager.Instance.SaveData();
            Debug.Log(selectedItem.name + " purchased!");
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void CheatCoins()
    {
        DataManager.Instance.appData.user.coins += 1000;
        DataManager.Instance.SaveData();
        Debug.Log("1000 coins added!");
    }
}