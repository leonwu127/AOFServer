using ArmyServer.Models;

namespace ArmyServer.Data;

public class ShopRepository : IDataRepository<string, ShopItem>
{
    private static readonly Dictionary<string, ShopItem> ShopItems = new();

    public void Add(string id, ShopItem model)
    {
        ShopItems.Add(id, model);
    }

    public void Set(string key, ShopItem model)
    {
        ShopItems[key] = model;
    }

    public ShopItem Get(string key)
    {
        return ShopItems[key];
    }

    public List<ShopItem> GetAll()
    {
        return ShopItems.Values.ToList();
    }

    public bool Remove(string key)
    {
        return ShopItems.Remove(key);
    }

    public bool Exists(string key)
    {
        return ShopItems.ContainsKey(key);
    }
}