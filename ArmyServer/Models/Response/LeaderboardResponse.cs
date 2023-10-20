namespace ArmyServer.Models.Response;

public class LeaderboardResponse
{ 
    public List<PlayerScore> PlayerScores { get; set; }

    public LeaderboardResponse(List<PlayerScore> playerScores)
    {
        PlayerScores = playerScores;
    }
}