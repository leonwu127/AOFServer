using System.Text.Json.Serialization;

namespace TinyGameServer.Models.Response;

public class AuthResponse
{
    
    public string Id { get; set; }
    public string Token { get; set; }
    
public AuthResponse(string id, string token)
    {
        Id = id;
        Token = token;
    }
}