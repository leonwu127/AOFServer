using TinyGameServer.Data;
using TinyGameServer.Models;
using TinyGameServer.Services.Leaderboard;
using Moq;
using Xunit;

namespace TinyGameServer.Tests.Services;

public class LeaderboardServiceTests
{
    private readonly Mock<IDataRepository<string, PlayerScore>> _leaderboardDataMock;
    private readonly ILeaderboardService _leaderboardService;
    
    public LeaderboardServiceTests()
    {
        _leaderboardDataMock = new Mock<IDataRepository<string, PlayerScore>>();
        _leaderboardService = new LeaderboardService(_leaderboardDataMock.Object);
    }
    
    [Fact]
    public void GetTopPlayers_ShouldReturnTop100Players()
    {
        // Arrange
        var expectedPlayers = new List<PlayerScore>();
        // Add 200 players
        // give everyplayer a sequential score
        for (int i = 0; i < 200; i++)
        {
            expectedPlayers.Add(new PlayerScore($"playerId{i}", $"playerName{i}", i));
        }
        _leaderboardDataMock.Setup(l => l.GetAll()).Returns(expectedPlayers);
        
        // Act
        var actualPlayers = _leaderboardService.GetTopPlayers();
        
        // Assert
        Assert.Equal(100, actualPlayers.Count);
        Assert.Equal(actualPlayers[0].Score, 199);
    }
    
    [Fact]
    public void AddOrUpdatePlayerScore_NewPlayer_ShouldAddPlayer()
    {
        // Arrange
        string playerId = "playerId";
        string playerName = "playerName";
        int newScore = 100;
        _leaderboardDataMock.Setup(l => l.Get(playerId)).Returns((PlayerScore) null);
        
        // Act
        _leaderboardService.AddOrUpdatePlayerScore(playerId, playerName, newScore);
        
        // Assert
        _leaderboardDataMock.Verify(l => l.Add(playerId, It.IsAny<PlayerScore>()), Times.Once);
    }
    
    
    [Fact]
    public void AddOrUpdatePlayerScore_ExistingPlayer_ShouldUpdatePlayer()
    {
        // Arrange
        string playerId = "playerId";
        string playerName = "playerName";
        int newScore = 100;
        var playerScore = new PlayerScore(playerId, playerName, 0);
        _leaderboardDataMock.Setup(l => l.Get(playerId)).Returns(playerScore);
        
        // Act
        _leaderboardService.AddOrUpdatePlayerScore(playerId, playerName, newScore);
        
        // Assert
        _leaderboardDataMock.Verify(l => l.Set(playerId, It.IsAny<PlayerScore>()), Times.Once);
    }
    
}