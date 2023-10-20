using ArmyServer.Data;
using ArmyServer.Models;

namespace ArmyServer.Services.Leaderboard;

public class LeaderboardService : ILeaderboardService
{
    private readonly IDataRepository<string, PlayerScore> _leaderboardRepository;
    
    public LeaderboardService(IDataRepository<string, PlayerScore> leaderboardRepository)
    {
        _leaderboardRepository = leaderboardRepository;
    }

    public List<PlayerScore> GetTopPlayers()
    {
        return _leaderboardRepository.GetAll().OrderByDescending(p => p.Score).Take(100).ToList();
    }

    public void AddOrUpdatePlayerScore(string playerId, string name, int newScore)
    {
        var playerScore = _leaderboardRepository.Get(playerId);
        if (playerScore == null)
        {
            playerScore = new PlayerScore(playerId, name, newScore);
            _leaderboardRepository.Add(playerId, playerScore);
        }
        else
        {
            playerScore.Score = newScore;
            _leaderboardRepository.Set(playerId, playerScore);
        }
    }
}