using ArmyServer.Models;

namespace ArmyServer.Services.Leaderboard;

public interface ILeaderboardService
{
    List<PlayerScore> GetTopPlayers();
    void AddOrUpdatePlayerScore(string playerId, string name, int newScore);
}