using TinyGameServer.Models;

namespace TinyGameServer.Services.Shop
{
    public interface IShopService
    {
        List<ShopItem> GetAllShopItems();
        bool PurchaseShopItem(string playerId, string itemId);
    }
}