using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ShopItemLoader : MonoBehaviour
{
    public Transform content;
    public GameObject shopItemButton;
    public List<ShopItem> shopItems;

    void Start()
    {
        Debug.Log("ShopItemLoader Start, item count: " + shopItems.Count);
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
        Debug.Log("Instantiated: " + newItem.name + " parent: " + newItem.transform.parent.name);
        
        TMP_Text nameText = newItem.transform.Find("InfoPanel/Name").GetComponent<TMP_Text>();
        TMP_Text priceText = newItem.transform.Find("InfoPanel/Price/PriceText").GetComponent<TMP_Text>();
        Image img = newItem.transform.Find("Image").GetComponent<Image>();

        nameText.text = item.name;
        priceText.text = item.price + " coins";
        img.sprite = item.image;

        Button button = newItem.GetComponent<Button>();
        button.onClick.AddListener(() => {
            FindObjectOfType<CheckoutPanel>().SelectItem(item);
        });
    }
}
