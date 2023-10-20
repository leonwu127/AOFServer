using System.Collections.Concurrent;
using ArmyServer.Models;

namespace ArmyServer.Data;

public class LeaderboardRepository : IDataRepository<string, PlayerScore>
{
    private static readonly ConcurrentDictionary<string, PlayerScore> Leaderboard = new();
    public void Add(string key, PlayerScore model)
    {
        Leaderboard.TryAdd(key, model);
    }

    public void Set(string key, PlayerScore model)
    {
        Leaderboard.TryUpdate(key, model, Leaderboard[key]);
    }

    public PlayerScore? Get(string key)
    {
        return Leaderboard[key];
    }

    public List<PlayerScore> GetAll()
    {
        return Leaderboard.Values.ToList();
    }

    public bool Remove(string key)
    {
        return Leaderboard.TryRemove(key, out _);
    }

    public bool Exists(string key)
    {
        return Leaderboard.ContainsKey(key);
    }
}