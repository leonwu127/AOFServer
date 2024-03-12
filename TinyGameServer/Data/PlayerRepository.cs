using TinyGameServer.Models;

namespace TinyGameServer.Data;

public class PlayerRepository : IDataRepository<string, Player>
{
    private static readonly Dictionary<string, Player> Players = new();

    public void Add(string id, Player player)
    {
        Players.Add(id, player);
    }

    public void Set(string key, Player model)
    {
        Players[key] = model;
    }

    public Player? Get(string id)
    {
        return Players.GetValueOrDefault(id);
    }

    public List<Player> GetAll()
    {
        return Players.Values.ToList();
    }

    public bool Remove(string id)
    {
        return Players.Remove(id);
    }

    public bool Exists(string id)
    {
        return Players.ContainsKey(id);
    }
}