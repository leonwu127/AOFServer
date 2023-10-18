using System.Net;
using System.Text.Json;
using ArmyServer.Controllers;
using ArmyServer.Models;
using ArmyServer.Services.Shop;
using ArmyServer.Utilities;
using ArmyServer.Utilities.HttpListenserWrapper;
using Moq;
using Xunit;
using static System.Text.Encoding;

namespace ArmyServer.Tests;

public class ShopControllerTests
{
    private readonly Mock<IShopService> _shopServiceMock;
    private readonly ShopController _controller;
    private readonly Mock<IHttpListenerRequestWrapper> _requestMock;
    private readonly Mock<IHttpListenerResponseWrapper> _responseMock;
    private const string PlayerId = "player_id";
    private const GameTitle GameTitle = Models.GameTitle.ArmyOfTactics;
    private const Platform Platform = Models.Platform.iOS;
    private static readonly string Token = TokenUtility.GenerateToken(PlayerId);
    private static readonly string AuthToken = "Bearer " + Token;
    private static readonly ShopItem ShopItem1 = new("shop_item_id", "shop_item_name", 100, 0.99m);
    
    public ShopControllerTests()
    {
        _shopServiceMock = new Mock<IShopService>();
        _controller = new ShopController(_shopServiceMock.Object);
        _requestMock = new Mock<IHttpListenerRequestWrapper>();
        _responseMock = new Mock<IHttpListenerResponseWrapper>();
    }
    
    [Fact]
    public void GetShopItems_ValidToken_SendsShopItemsList()
    {
        // Arrange
        var fakeShopItemsList = new List<ShopItem> { ShopItem1 };
        _shopServiceMock.Setup(service => service.GetAllShopItems()).Returns(fakeShopItemsList);
        var responseStream = new MemoryStream();
        _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
        _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
        {
            { "Authorization", AuthToken },
            { "platform", Platform.ToString() }
        });
        
        // Act
        _controller.GetShopItems(_requestMock.Object, _responseMock.Object);

        // Assert
        responseStream.Position = 0;
        var reader = new StreamReader(responseStream);
        var jsonResponse = reader.ReadToEnd();
        Assert.Contains("Items", jsonResponse);
    }
    
    [Fact]
    public void PurchaseItem_ValidToken_SendsOkResponse()
    {
        // Arrange
        var responseStream = new MemoryStream();
        _responseMock.Setup(resp => resp.OutputStream).Returns(responseStream);
        _requestMock.Setup(req => req.Headers).Returns(new WebHeaderCollection
        {
            { "Authorization", AuthToken },
            { "platform", Platform.ToString() }
        });
        int? capturedStatusCode = null;
        _responseMock
            .SetupSet(r => r.StatusCode = It.IsAny<int>())
            .Callback<int>(value => capturedStatusCode = value);
        _requestMock.Setup(req => req.Url).Returns(new Uri("http://localhost:8080/ArmyofTactics/shop/purchase"));
        _requestMock.Setup(req => req.HttpMethod).Returns("POST");
        _requestMock.Setup(req => req.InputStream).Returns(new MemoryStream(UTF8.GetBytes(JsonSerializer.Serialize(new Dictionary<string, string>
        {
            { "itemId", ShopItem1.ItemId }
        }))));
        _shopServiceMock.Setup(service => service.PurchaseShopItem(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        
        // Act
        _controller.PurchaseItem(_requestMock.Object, _responseMock.Object);

        // Assert
        Assert.NotNull(capturedStatusCode);
        Assert.Equal((int)HttpStatusCode.OK, capturedStatusCode); 
    }
    
}