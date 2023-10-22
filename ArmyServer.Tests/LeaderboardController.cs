using System.Net;
using System.Text;
using ArmyServer.Controllers;
using ArmyServer.Models;
using ArmyServer.Services.Leaderboard;
using ArmyServer.Utilities;
using ArmyServer.Utilities.HttpListenserWrapper;
using Moq;
using Newtonsoft.Json;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ArmyServer.Tests;

public class LeaderboardControllerTests
{
    private readonly Mock<ILeaderboardService> _leaderboardServiceMock;
    private readonly LeaderboardController _controller;
    private readonly Mock<IHttpListenerRequestWrapper> _requestMock;
    private readonly Mock<IHttpListenerResponseWrapper> _responseMock;
    private const string PlayerId = "player_id";
    private const string PlayerName = "player_name";
    private const int Score = 100;
    private static readonly PlayerScore PlayerScore1 = new(PlayerId, PlayerName, Score);
    private const GameTitle GameTitle = Models.GameTitle.ArmyOfTactics;
    private const Platform Platform = Models.Platform.iOS;
    private static readonly string Token = TokenUtility.GenerateToken(PlayerId);
    private static readonly string AuthToken = "Bearer " + Token;
    
    
    public LeaderboardControllerTests()
    {
        _leaderboardServiceMock = new Mock<ILeaderboardService>();
        _controller = new LeaderboardController(_leaderboardServiceMock.Object);
        _requestMock = new Mock<IHttpListenerRequestWrapper>();
        _responseMock = new Mock<IHttpListenerResponseWrapper>();
    }
    
    [Fact]
    public void GetTopPlayers_ValidToken_SendsTopPlayersList()
    {
        // Arrange
        var fakeTopPlayersList = new List<PlayerScore> { new(PlayerId, "player_name", 100) };
        _leaderboardServiceMock.Setup(service => service.GetTopPlayers()).Returns(fakeTopPlayersList);
        var responseStream = new MemoryStream();
        _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
        _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
        {
            { "Authorization", AuthToken },
            { "platform", Platform.ToString() }
        });
        
        // Act
        _controller.GetTopPlayers(_requestMock.Object, _responseMock.Object);

        // Assert
        responseStream.Position = 0;
        var reader = new StreamReader(responseStream);
        var jsonResponse = reader.ReadToEnd();
        Assert.Contains("PlayerScores", jsonResponse);
    }
    
    [Fact]
    public void AddOrUpdatePlayerScore_ValidToken_SendsOkResponse()
    {
        // Arrange
        var requestBody = JsonSerializer.Serialize(PlayerScore1) ;
        var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
        var responseStream = new MemoryStream();
        _requestMock.Setup(req => req.InputStream).Returns(requestStream);
        _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
        _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
        {
            { "Authorization", AuthToken },
            { "platform", Platform.ToString() }
        });
        
        // Act
        _controller.AddOrUpdatePlayerScore(_requestMock.Object, _responseMock.Object);

        // Assert
        responseStream.Position = 0;
        var reader = new StreamReader(responseStream);
        var jsonResponse = reader.ReadToEnd();
        _leaderboardServiceMock.Verify(service => service.AddOrUpdatePlayerScore(PlayerId, PlayerName, Score));
    }
    
    
}