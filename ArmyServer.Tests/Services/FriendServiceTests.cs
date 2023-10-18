using ArmyServer.Data;
using ArmyServer.Models;
using ArmyServer.Services.Friends;
using Moq;
using Xunit;

namespace ArmyServer.Tests.Services;

public class FriendServiceTests
{
    private readonly Mock<IDataRepository<string, List<Friend>>> _friendDataMock;
    private readonly FriendsService _friendService;
    
    public FriendServiceTests()
    {
        _friendDataMock = new Mock<IDataRepository<string, List<Friend>>>();
        _friendService = new FriendsService(_friendDataMock.Object);
    }
    
    [Fact]
    public void GetFriends_NullPlayerId_ShouldReturnNull()
    {
        // Arrange
        string playerId = null;
        
        // Act
        var actualResult = _friendService.GetFriends(playerId);
        
        // Assert
        Assert.Null(actualResult);
    }
    
    [Fact]
    public void GetFriends_ValidPlayerId_ShouldReturnFriends()
    {
        // Arrange
        string playerId = "playerId";
        var expectedFriends = new List<Friend>();
        _friendDataMock.Setup(f => f.Get(playerId)).Returns(expectedFriends);
        
        // Act
        var actualResult = _friendService.GetFriends(playerId);
        
        // Assert
        Assert.Equal(expectedFriends, actualResult);
    }
    
    [Fact]
    public void AddFriend_ValidPlayerId_ShouldAddFriend()
    {
        // Arrange
        string playerId = "playerId";
        var newFriend = new Friend("friendId", "friendName");
        List<Friend> friends = new List<Friend>();
        _friendDataMock.Setup(f => f.Get(playerId)).Returns(friends);
        
        // Act
        _friendService.AddFriend(playerId, newFriend);
        
        // Assert
        _friendDataMock.Verify(f => f.Set(playerId, friends), Times.Once);
        Assert.Contains(newFriend, friends);
    }
    
    [Fact]
    public void RemoveFriend_NullPlayerId_ShouldNotRemoveFriend()
    {
        // Arrange
        string playerId = null;
        string friendId = "friendId";
        
        // Act
        var actualResult = _friendService.RemoveFriend(playerId, friendId);
        
        // Assert
        Assert.False(actualResult);
        _friendDataMock.Verify(f => f.Set(It.IsAny<string>(), It.IsAny<List<Friend>>()), Times.Never);
    }
    
    [Fact]
    public void RemoveFriend_NullFriendId_ShouldNotRemoveFriend()
    {
        // Arrange
        string playerId = "playerId";
        string friendId = null;
        
        // Act
        var actualResult = _friendService.RemoveFriend(playerId, friendId);
        
        // Assert
        Assert.False(actualResult);
        _friendDataMock.Verify(f => f.Set(It.IsAny<string>(), It.IsAny<List<Friend>>()), Times.Never);
    }
    
    [Fact]
    public void RemoveFriend_ValidPlayerIdAndFriendId_ShouldRemoveFriend()
    {
        // Arrange
        string playerId = "playerId";
        string friendId = "friendId";
        var friendToRemove = new Friend(friendId, "friendName");
        List<Friend> friends = new List<Friend> {friendToRemove};
        _friendDataMock.Setup(f => f.Get(playerId)).Returns(friends);
        
        // Act
        var actualResult = _friendService.RemoveFriend(playerId, friendId);
        
        // Assert
        Assert.True(actualResult);
        _friendDataMock.Verify(f => f.Set(playerId, friends), Times.Once);
        Assert.DoesNotContain(friendToRemove, friends);
    }
}