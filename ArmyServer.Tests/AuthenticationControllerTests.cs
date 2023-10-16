using ArmyServer.Auth;
using ArmyServer.Controllers;
using ArmyServer.Models;
using System.Net;
using System.Text;
using Xunit;

namespace ArmyServer.Tests
{
    public class ArmyServerTests : IDisposable
    {
        private const string TestUrl = "http://localhost:12345/";
        private string _gameTitle = GameTitle.ArmyofTactics.ToString();
        private readonly HttpListener _listener;
        private readonly HttpClient _client;
        private readonly AuthenticationController _controller;
        public ArmyServerTests()
        {
            // Initialize the controller. If the controller has dependencies, mock them here.
            _listener = new HttpListener();
            _listener.Prefixes.Add(TestUrl);
            _listener.Start();

            _client = new HttpClient();
            _controller = new AuthenticationController();
        }

        [Fact]
        public async Task GuestRegister_ShouldReturnToken()
        {
            // Arrange
            var requestUrl = $"{TestUrl}register";

            // Start a task to handle the incoming request
            var handleRequestTask = Task.Run(async () =>
            {
                var listenerContext = await _listener.GetContextAsync();
                _controller.GuestRegister(listenerContext.Request, listenerContext.Response);
                listenerContext.Response.Close();
            });

            // Act
            var httpResponse = await _client.GetAsync(requestUrl);

            // Wait for the request handling task to complete
            await handleRequestTask;

            // Assert
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            Assert.Contains("token", responseContent); // Example assertion

        }

        [Fact]
        public async Task Login_ShouldReturnToken()
        {
            // Arrange
            var requestUrl = $"{TestUrl}{_gameTitle}/login";
            var requestBodyContent = new
            {
                Provider = new Dictionary<string, AuthCredential>
                {
                    ["GameCenter"] = new AuthCredential
                    {
                        PlayerId = "testPlayer",
                        Token = "testToken"
                    }
                }
            };

            var requestBodyJson = System.Text.Json.JsonSerializer.Serialize(requestBodyContent);
            var requestBody = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            // Set headers
            _client.DefaultRequestHeaders.Add("platform", "iOS");

            // Start a task to handle the incoming request
            var handleRequestTask = Task.Run(async () =>
            {
                var listenerContext = await _listener.GetContextAsync();
                _controller.Login(listenerContext.Request, listenerContext.Response);
                listenerContext.Response.Close();
            });

            // Act
            var httpResponse = await _client.PostAsync(requestUrl, requestBody);

            // Wait for the request handling task to complete
            await handleRequestTask;

            // Assert
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            Assert.Contains("token", responseContent); // Example assertion
        }

        public void Dispose()
        {
            _listener.Stop();
            _listener.Close();
            _client.Dispose();
        }

    }
}