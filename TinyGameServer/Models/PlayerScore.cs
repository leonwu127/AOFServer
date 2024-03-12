namespace TinyGameServer.Models;

public class PlayerScore
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Score { get; set; }
    
    public PlayerScore(string id, string name, int score)
    {
        Id = id;
        Name = name;
        Score = score;
    }
    
}