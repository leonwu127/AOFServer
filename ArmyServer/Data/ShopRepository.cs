using ArmyServer.Models;

namespace ArmyServer.Data;

public class ShopRepository : IDataRepository<string, ShopItem>
{
    private readonly Dictionary<string, ShopItem> _shopItems;
    public void Add(string id, ShopItem model)
    {
        _shopItems.Add(id, model);
    }

    public void Set(string key, ShopItem model)
    {
        _shopItems[key] = model;
    }

    public ShopItem Get(string key)
    {
        return _shopItems[key];
    }

    public List<ShopItem> GetAll()
    {
        return _shopItems.Values.ToList();
    }

    public bool Remove(string key)
    {
        return _shopItems.Remove(key);
    }

    public bool Exists(string key)
    {
        return _shopItems.ContainsKey(key);
    }
}