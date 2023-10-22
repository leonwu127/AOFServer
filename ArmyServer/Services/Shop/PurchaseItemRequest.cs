namespace ArmyServer.Services.Shop;

public class PurchaseItemRequest
{
    public PurchaseItemRequest(string itemId)
    {
        ItemId = itemId;
    }

    public string ItemId { get; set; }
}