using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ShopItemLoader : MonoBehaviour
{
    public Transform content;
    public GameObject shopItemButton;
    public List<ShopItem> shopItems;

    void Start()
    {
        LoadItems();
    }

    public void LoadItems()
    {
        foreach (var item in shopItems)
        {
            AddListItem(item);
        }
    }

    private void AddListItem(ShopItem item)
{
    GameObject newItem = Instantiate(shopItemButton, content);
    
    TMP_Text nameText = newItem.transform.Find("Name").GetComponent<TMP_Text>();
    TMP_Text priceText = newItem.transform.Find("Price").GetComponent<TMP_Text>();

    nameText.text = item.name;
    priceText.text = item.price + " coins";
}
}
