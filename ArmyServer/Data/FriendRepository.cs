using System.Collections.Concurrent;
using ArmyServer.Models;

namespace ArmyServer.Data;

public class FriendRepository : IDataRepository<string, List<Friend>>
{
    private static readonly ConcurrentDictionary<string, List<Friend>> Friends = new();
    
    public void Add(string key,List<Friend> friends)
    {
        Friends.TryAdd(key, friends);
    }

    public void Set(string key, List<Friend> friends)
    {
        Friends.AddOrUpdate(key, friends, (_, _) => friends);
    }

    public List<Friend> Get(string key)
    {
        return Friends[key];
    }

    public bool Remove(string key)
    {
        return Friends.TryRemove(key, out _);
    }

    public bool Exists(string key)
    {
        return Friends.ContainsKey(key);
    }

    public List<List<Friend>> GetAll()
    {
        return Friends.Values.ToList();
    }
    
    
}