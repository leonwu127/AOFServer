using System.Text;
using Xunit;
using Moq;
using TinyGameServer.Controllers;
using TinyGameServer.Utilities.HttpListenserWrapper;
using System.Net;
using TinyGameServer.Services.Auth;
using System.Text.Json;
using TinyGameServer.Models;
using TinyGameServer.Data;
using TinyGameServer.Utilities;

namespace TinyGameServer.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IHttpListenerRequestWrapper> _requestMock;
        private readonly Mock<IHttpListenerResponseWrapper> _responseMock;
        private readonly Mock<IDataRepository<string, Player>> _playersDataMock;
        private readonly AuthenticationController _controller;
        private const GameTitle GameTitle = Models.GameTitle.ArmyOfTactics;
        private const Platform Platform = Models.Platform.iOS;
        private readonly Uri _loginUrl = new Uri("http://localhost:8080/ArmyofTactics/login");
        private readonly Uri _registerUrl = new Uri("http://localhost:8080/ArmyofTactics/register");
        private const string PlayerId = "player_id";
        private static readonly string Token = TokenUtility.GenerateToken(PlayerId);


        public AuthenticationControllerTests()
        {
            _playersDataMock = new Mock<IDataRepository<string, Player>>();
            _requestMock = new Mock<IHttpListenerRequestWrapper>();
            _responseMock = new Mock<IHttpListenerResponseWrapper>();
            _controller = new AuthenticationController(_playersDataMock.Object);
        }
        
        [Fact]
        public void GuestRegister_ShouldReturnToken()
        {
            // Arrange
            _requestMock.Setup(req => req.Url).Returns(_registerUrl);
            _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
            {
                { "GameTitle", GameTitle.ToString() },
                { "platform", Platform.ToString() }
            });
            _playersDataMock.Setup(playersData => playersData.Add(It.IsAny<string>(), It.IsAny<Player>()));

            // Prepare the response mock to capture the response data for verification
            var responseStream = new MemoryStream();
            _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);

            // Act
            _controller.GuestRegister(_requestMock.Object, _responseMock.Object);

            // Assert
            responseStream.Position = 0;
            var reader = new StreamReader(responseStream);
            var jsonResponse = reader.ReadToEnd();
            Assert.Contains("Id", jsonResponse);
            Assert.Contains("Token", jsonResponse);
        }
        

        [Fact]
        public void Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var authRequest = new AuthRequest
            {
                Provider = new Dictionary<string, AuthCredential>
                {
                    { "Guest", new AuthCredential(PlayerId, Token)}
                }
            };
            var requestBody = JsonSerializer.Serialize(authRequest);
            var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
            var responseStream = new MemoryStream();
            _requestMock.Setup(req => req.Url).Returns(_loginUrl);
            _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
            {
                { "GameTitle", GameTitle.ToString() },
                { "platform", Platform.ToString() }
            });
            _requestMock.Setup(req => req.InputStream).Returns(requestStream);
            _requestMock.Setup(req => req.ContentEncoding).Returns(Encoding.UTF8);
            _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
            _playersDataMock.Setup(playersData => playersData.Get(PlayerId)).Returns(new Player(PlayerId));
            
            // Act
            _controller.Login(_requestMock.Object, _responseMock.Object);

            // Assert
            responseStream.Position = 0;
            var reader = new StreamReader(responseStream);
            var jsonResponse = reader.ReadToEnd();
            Assert.Contains(PlayerId, jsonResponse);
            Assert.Contains(Token, jsonResponse);
        }
    }
}
