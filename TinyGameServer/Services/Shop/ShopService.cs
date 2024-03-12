using TinyGameServer.Data;
using TinyGameServer.Models;

namespace TinyGameServer.Services.Shop
{
    public class ShopService : IShopService
    {
        private readonly IDataRepository<string, ShopItem> _shopData;
        private readonly IDataRepository<string, Player> _playerData;
        
        public ShopService(IDataRepository<string, ShopItem> shopData, IDataRepository<string, Player> playerData)
        {
            _shopData = shopData;
            _playerData = playerData;
        }
        
        public List<ShopItem> GetAllShopItems()
        {
            return _shopData.GetAll();
        }

        public bool PurchaseShopItem(string playerId, string itemId)
        {
            ShopItem itemToPurchase = _shopData.Get(itemId);
            if (itemToPurchase == null) return false;
            
            Player player = _playerData.Get(playerId);
            if (player == null) return false;
            
            if (player.Gold < itemToPurchase.Price) return false;
            
            player.Gold -= itemToPurchase.Price;
            player.Inventory.Add(itemToPurchase);
            
            itemToPurchase.Amount--;
            if (itemToPurchase.Amount <= 0)
            {
                _shopData.Remove(itemId);
            }
            _shopData.Set(itemId, itemToPurchase);
            _playerData.Set(playerId, player);
            return true;
        }
    }
}