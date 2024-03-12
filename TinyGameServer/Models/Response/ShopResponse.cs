using System.Text.Json.Serialization;

namespace TinyGameServer.Models.Response;

public class ShopResponse
{
    [JsonPropertyName("Items")]
    public List<ShopItem> Items { get; set; }

    public ShopResponse(List<ShopItem> items)
    {
        Items = items;
    }
}