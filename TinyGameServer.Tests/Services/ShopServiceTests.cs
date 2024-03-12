using TinyGameServer.Data;
using TinyGameServer.Models;
using TinyGameServer.Services.Shop;
using Moq;
using Xunit;

namespace TinyGameServer.Tests.Services;

public class ShopServiceTests
{
    private readonly Mock<IDataRepository<string, ShopItem>> _shopDataMock = new();
    private readonly Mock<IDataRepository<string, Player>> _playerDataMock = new();
    private readonly ShopService _shopService;
    
    public ShopServiceTests()
    {
        _shopService = new ShopService(_shopDataMock.Object, _playerDataMock.Object);
    }
    
    [Fact]
    public void GetAllShopItems_ShouldReturnAllShopItems()
    {
        // Arrange
        var expectedShopItems = new List<ShopItem>();
        _shopDataMock.Setup(s => s.GetAll()).Returns(expectedShopItems);
        
        // Act
        var actualShopItems = _shopService.GetAllShopItems();
        
        // Assert
        Assert.Equal(expectedShopItems, actualShopItems);
    }
    
    [Fact]
    public void PurchaseShopItem_InvalidItemId_ShouldReturnFalse()
    {
        // Arrange
        string playerId = "playerId";
        string itemId = "itemId";
        _shopDataMock.Setup(s => s.Get(itemId)).Returns((ShopItem) null);
        
        // Act
        var actualResult = _shopService.PurchaseShopItem(playerId, itemId);
        
        // Assert
        Assert.False(actualResult);
    }
    
    [Fact]
    public void PurchaseShopItem_InvalidPlayerId_ShouldReturnFalse()
    {
        // Arrange
        string playerId = "playerId";
        string itemId = "itemId";
        var shopItem = new ShopItem(itemId, "itemName", 100, 100m);

        _shopDataMock.Setup(s => s.Get(itemId)).Returns(shopItem);
        _playerDataMock.Setup(p => p.Get(playerId)).Returns((Player) null);
        
        // Act
        var actualResult = _shopService.PurchaseShopItem(playerId, itemId);
        
        // Assert
        Assert.False(actualResult);
    }
    
    [Fact]
    public void PurchaseShopItem_NotEnoughGold_ShouldReturnFalse()
    {
        // Arrange
        string playerId = "playerId";
        string itemId = "itemId";
        var shopItem = new ShopItem(itemId, "itemName", 100, 100m);
        var player = new Player(playerId, 50);
        _shopDataMock.Setup(s => s.Get(itemId)).Returns(shopItem);
        _playerDataMock.Setup(p => p.Get(playerId)).Returns(player);
        
        // Act
        var actualResult = _shopService.PurchaseShopItem(playerId, itemId);
        
        // Assert
        Assert.False(actualResult);
    }
    
    [Fact]
    public void PurchaseShopItem_ValidPurchase_ShouldReturnTrue()
    {
        // Arrange
        string playerId = "playerId";
        string itemId = "itemId";
        var shopItem = new ShopItem(itemId, "itemName", 100, 100m);
        var player = new Player(playerId, 100);
        _shopDataMock.Setup(s => s.Get(itemId)).Returns(shopItem);
        _playerDataMock.Setup(p => p.Get(playerId)).Returns(player);
        
        // Act
        var actualResult = _shopService.PurchaseShopItem(playerId, itemId);
        
        // Assert
        Assert.True(actualResult);
        _playerDataMock.Verify(p => p.Set(playerId, It.IsAny<Player>()), Times.Once);
        Assert.Equal(0, player.Gold);
        Assert.Contains(shopItem, player.Inventory);
        Assert.Equal(99, shopItem.Amount);
    }
    
    [Fact]
    public void PurchaseShopItem_LastItem_ShouldRemoveItem()
    {
        // Arrange
        string playerId = "playerId";
        string itemId = "itemId";
        var shopItem = new ShopItem(itemId, "itemName", 1, 100m);
        var player = new Player(playerId, 100);
        _shopDataMock.Setup(s => s.Get(itemId)).Returns(shopItem);
        _playerDataMock.Setup(p => p.Get(playerId)).Returns(player);
        
        // Act
        var actualResult = _shopService.PurchaseShopItem(playerId, itemId);
        
        // Assert
        Assert.True(actualResult);
        _playerDataMock.Verify(p => p.Set(playerId, It.IsAny<Player>()), Times.Once);
        Assert.Equal(0, player.Gold);
        Assert.Contains(shopItem, player.Inventory);
        Assert.Equal(0, shopItem.Amount);
        _shopDataMock.Verify(s => s.Remove(itemId), Times.Once);
    }
}