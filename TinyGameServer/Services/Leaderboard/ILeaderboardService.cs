using TinyGameServer.Models;

namespace TinyGameServer.Services.Leaderboard;

public interface ILeaderboardService
{
    List<PlayerScore> GetTopPlayers();
    void AddOrUpdatePlayerScore(string playerId, string name, int newScore);
}