using ArmyServer.Models;

namespace ArmyServer.Services.Shop
{
    public interface IShopService
    {
        List<ShopItem> GetAllShopItems();
        bool PurchaseShopItem(string playerId, string itemId);
    }
}