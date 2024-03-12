using TinyGameServer.Data;
using TinyGameServer.Models;
using TinyGameServer.Services.Auth;
using TinyGameServer.Services.Auth.Provider;
using Moq;
using Xunit;

namespace TinyGameServer.Tests.Services;

public class GuestAuthServicesTests
{
    private readonly Mock<IDataRepository<string, Player>> _playerDataMock;
    private readonly GuestAuthentication _guestAuthServices;
    
    public GuestAuthServicesTests()
    {
        _playerDataMock = new Mock<IDataRepository<string, Player>>();
        _guestAuthServices = new GuestAuthentication(_playerDataMock.Object);
    }
    
    [Fact]
    public void Register_GuestPlayer_ShouldReturnNewPlayer()
    {
        // Arrange
        _playerDataMock.Setup(p => p.Add(It.IsAny<string>(), It.IsAny<Player>()));
        
        // Act
        var actualPlayer = _guestAuthServices.Authenticate();
        
        // Assert
        Assert.Contains("Guest_", actualPlayer.Id);
        _playerDataMock.Verify(p => p.Add(It.IsAny<string>(), It.IsAny<Player>()), Times.Once);
    }
    
    [Fact]
    public void Authenticate_NullAuthCredential_ShouldReturnFalse()
    {
        // Arrange
        AuthCredential authCredential = null;
        
        // Act
        var actualResult = _guestAuthServices.Authenticate(authCredential, out var actualPlayer);
        
        // Assert
        Assert.False(actualResult);
        Assert.Null(actualPlayer);
    }
    
    [Fact]
    public void Authenticate_NullPlayerId_ShouldReturnFalse()
    {
        // Arrange
        AuthCredential authCredential = new AuthCredential(null, "token");
        
        // Act
        var actualResult = _guestAuthServices.Authenticate(authCredential, out var actualPlayer);
        
        // Assert
        Assert.False(actualResult);
        Assert.Null(actualPlayer);
    }
    
    [Fact]
    public void Authenticate_NullToken_ShouldReturnFalse()
    {
        // Arrange
        AuthCredential authCredential = new AuthCredential("playerId", null);
        
        // Act
        var actualResult = _guestAuthServices.Authenticate(authCredential, out var actualPlayer);
        
        // Assert
        Assert.False(actualResult);
        Assert.Null(actualPlayer);
    }
    
    [Fact]
    public void Authenticate_InvalidToken_ShouldReturnFalse()
    {
        // Arrange
        AuthCredential authCredential = new AuthCredential("playerId", "token");
        
        // Act
        var actualResult = _guestAuthServices.Authenticate(authCredential, out var actualPlayer);
        
        // Assert
        Assert.False(actualResult);
        Assert.Null(actualPlayer);
    }
    
    [Fact]
    public void Authenticate_PlayerNotFound_ShouldReturnFalse()
    {
        // Arrange
        AuthCredential authCredential = new AuthCredential("playerId", "token");
        _playerDataMock.Setup(p => p.Get(It.IsAny<string>())).Returns((Player)null);
        
        // Act
        var actualResult = _guestAuthServices.Authenticate(authCredential, out var actualPlayer);
        
        // Assert
        Assert.False(actualResult);
        Assert.Null(actualPlayer);
    }
    
    [Fact]
    public void Authenticate_ValidAuthCredential_ShouldReturnTrue()
    {
        // Arrange
        string token = Utilities.TokenUtility.GenerateToken("playerId");
        AuthCredential authCredential = new AuthCredential("playerId", token);
        _playerDataMock.Setup(p => p.Get(It.IsAny<string>())).Returns(new Player("playerId"));
        
        // Act
        var actualResult = _guestAuthServices.Authenticate(authCredential, out var actualPlayer);
        
        // Assert
        Assert.True(actualResult);
        Assert.NotNull(actualPlayer);
    }
}