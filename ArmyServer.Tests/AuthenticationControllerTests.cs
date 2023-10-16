using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using ArmyServer.Controllers;
using ArmyServer.Utilities.HttpListenserWrapper;
using System.Net;
using System.Security.Principal;
using ArmyServer.Services.Auth;
using System.Text.Json;
using ArmyServer.Models;
using ArmyServer.Services.Auth.Provider;
using ArmyServer.Data;
using ArmyServer.Utilities;

namespace ArmyServer.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IHttpListenerRequestWrapper> _requestMock;
        private readonly Mock<IHttpListenerResponseWrapper> _responseMock;
        private readonly AuthenticationController _controller;
        private const GameTitle GameTitle = Models.GameTitle.ArmyofTactics;
        private const Platform Platform = Models.Platform.iOS;
        private readonly Uri _url = new Uri("http://localhost:8080/ArmyofTactics/login");
        private const string PlayerId = "player_id";
        private static readonly string Token = TokenUtility.GenerateToken(PlayerId);


        public AuthenticationControllerTests()
        {
            _requestMock = new Mock<IHttpListenerRequestWrapper>();
            _responseMock = new Mock<IHttpListenerResponseWrapper>();
            _controller = new AuthenticationController();
            PlayersData.Add(new Player(PlayerId));
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
            _requestMock.Setup(req => req.Url).Returns(_url);
            _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
            {
                { "platform", Platform.ToString() }
            });
            _requestMock.Setup(req => req.InputStream).Returns(requestStream);
            _requestMock.Setup(req => req.ContentEncoding).Returns(Encoding.UTF8);

            // You might need to set up more properties of the request based on how they're used in your Login method

            // Prepare the response mock to capture the response data for verification
            var responseStream = new MemoryStream();
            _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);

            // Act
            _controller.Login(_requestMock.Object, _responseMock.Object);

            // Assert
            responseStream.Position = 0;
            var reader = new StreamReader(responseStream);
            var jsonResponse = reader.ReadToEnd();
            Assert.Contains("token", jsonResponse); // Adjust based on how your token is included in the response
        }
    }
}
