using System.Text.Json.Serialization;

namespace ArmyServer.Excetions;

public class ErrorResponse
{
    [JsonPropertyName("error")]
    public string Message { get; set; }
    
    public ErrorResponse(string message)
    {
        Message = message;
    }
}