using System.Net;
using System.Text;
using System.Text.Json;
using TinyGameServer.Controllers;
using TinyGameServer.Models;
using TinyGameServer.Services.Friends;
using TinyGameServer.Utilities;
using TinyGameServer.Utilities.HttpListenserWrapper;
using Moq;
using Xunit;

namespace TinyGameServer.Tests
{
    public class FriendsControllerTests
    {
        private readonly Mock<IFriendsService> _friendsServiceMock;
        private readonly FriendsController _controller;
        private readonly Mock<IHttpListenerRequestWrapper> _requestMock;
        private readonly Mock<IHttpListenerResponseWrapper> _responseMock;
        private const string FriendId = "friend_id";
        private const string FriendName = "friend_name";
        private const string PlayerId = "player_id";
        private const GameTitle GameTitle = Models.GameTitle.ArmyOfTactics;
        private const Platform Platform = Models.Platform.iOS;
        private static readonly string Token = TokenUtility.GenerateToken(PlayerId);
        private static readonly string AuthToken = "Bearer " + Token;
        private static readonly Friend Friend1 = new(FriendId, FriendName);
        
        
        public FriendsControllerTests()
        {
            _friendsServiceMock = new Mock<IFriendsService>();
            _controller = new FriendsController(_friendsServiceMock.Object);

            _requestMock = new Mock<IHttpListenerRequestWrapper>();
            _responseMock = new Mock<IHttpListenerResponseWrapper>();
        }
        
        [Fact]
        public void GetFriends_ValidToken_SendsFriendsList()
        {
            // Arrange
            var fakeFriendsList = new List<Friend> { Friend1 };
            _friendsServiceMock.Setup(service => service.GetFriends(It.IsAny<string>())).Returns(fakeFriendsList);
            var responseStream = new MemoryStream();
            _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
            _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
            {
                { "Authorization", AuthToken },
                { "platform", Platform.ToString() }
            });
            
            // Act
            _controller.GetFriends(_requestMock.Object, _responseMock.Object);

            // Assert
            responseStream.Position = 0;
            var reader = new StreamReader(responseStream);
            var jsonResponse = reader.ReadToEnd();
            Assert.Contains("friends", jsonResponse);
            var friendResponse = JsonSerializer.Deserialize<FriendResponse>(jsonResponse);
            Assert.Equal(fakeFriendsList[0].Id, friendResponse.Friends[0].Id);
        }
        
        [Fact]
        public void AddFriend_ValidData_AddsFriend()
        {
            // Arrange
            var requestBody = JsonSerializer.Serialize(Friend1);
            var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            var responseStream = new MemoryStream();
            _requestMock.Setup(req => req.InputStream).Returns(requestStream);
            _requestMock.Setup(req => req.ContentEncoding).Returns(Encoding.UTF8);
            _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
            _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
            {
                { "Authorization", AuthToken },
                { "platform", Platform.ToString() },
            });

            // Act
            _controller.AddFriend(_requestMock.Object, _responseMock.Object);

            // Assert
            responseStream.Position = 0;
            var reader = new StreamReader(responseStream);
            var jsonResponse = reader.ReadToEnd();
            _friendsServiceMock.Verify(service => service.AddFriend(It.IsAny<string>(), It.IsAny<Friend>()), Times.Once);
        } 
        
        [Fact]
        public void RemoveFriend_ValidData_RemovesFriend()
        {
            // Arrange
            
            var requestBody = JsonSerializer.Serialize(Friend1);
            var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            var responseStream = new MemoryStream();
            _requestMock.Setup(req => req.InputStream).Returns(requestStream);
            _requestMock.Setup(req => req.ContentEncoding).Returns(Encoding.UTF8);
            _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);

            _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
            {
                { "Authorization", AuthToken },
                { "platform", Platform.ToString() },
            });

            // Act
            _controller.RemoveFriend(_requestMock.Object, _responseMock.Object);

            // Assert
            _friendsServiceMock.Verify(service => service.RemoveFriend(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
