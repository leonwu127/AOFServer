using System.Text.Json.Serialization;

namespace ArmyServer.Models;

public class FriendResponse
{
    [JsonPropertyName("friends")]
    public List<Friend> Friends { get; set; }
    
    public FriendResponse(List<Friend> friends)
    {
        Friends = friends;
    }
}