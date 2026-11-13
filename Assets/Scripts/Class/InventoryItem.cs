// Ein gekauftes Item im Inventar des Users
[System.Serializable]
public class InventoryItem
{
    public string itemName;  // Verweis auf ShopItem.name
    public int quantity;     // wie viele der User davon hat
}