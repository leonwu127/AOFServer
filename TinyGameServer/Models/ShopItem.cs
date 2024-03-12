namespace TinyGameServer.Models;

public class ShopItem
{
    public string ItemId { get; set; }
    public string ItemName { get; set; }
    public int Amount { get; set; }
    public decimal Price { get; set; }
    
    public ShopItem(string itemId, string itemName, int amount, decimal price)
    {
        ItemId = itemId;
        ItemName = itemName;
        Amount = amount;
        Price = price;
    }
}