using ArmyServer.Excetions;
using ArmyServer.Models;
using ArmyServer.Services.Auth;
using ArmyServer.Services.Shop;
using ArmyServer.Utilities.HttpListenserWrapper;

namespace ArmyServer.Utilities.HttpUtilities;

public static class HttpUtility
{
    public static bool TryExtractAuthCredential(IHttpListenerRequestWrapper req, 
                                                out string provider, 
                                                out AuthCredential? authCredential,
                                                out string msg)
    {
        string requestBody= ExtractRequestBody(req);
        try
        {
            var parsedBody = System.Text.Json.JsonSerializer.Deserialize<AuthRequest>(requestBody);
            if (parsedBody.Provider.Count > 0)
            {
                provider = parsedBody.Provider.Keys.First();
                authCredential = parsedBody.Provider[provider];
                msg = string.Empty;
                return true;
            }
            authCredential = null;
            provider = null;
            msg = "Failed to parse request body.";
            return false;
        }
        catch (Exception e)
        {
            authCredential = null;
            provider = null;
            msg = $"Failed to parse request body: {e.Message}";
            return false;
        }
    }
    
    public static bool TryExtractFriend(IHttpListenerRequestWrapper req, out Friend? friend)
    {
        string requestBody= ExtractRequestBody(req);
        var parsedBody = System.Text.Json.JsonSerializer.Deserialize<Friend>(requestBody);
        if (parsedBody != null)
        {
            friend = parsedBody;
            return true;
        }
        friend = null;
        return false;
    }
    
    private static string ExtractRequestBody(IHttpListenerRequestWrapper req)
    {
        string requestBody;
        using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
        { requestBody = reader.ReadToEnd(); }
        return requestBody;
    }
    
    public static bool TryExtractPlayerScore(IHttpListenerRequestWrapper req, out PlayerScore? playerScore)
    {
        string requestBody= ExtractRequestBody(req);
        var parsedBody = System.Text.Json.JsonSerializer.Deserialize<PlayerScore>(requestBody);
        if (parsedBody != null)
        {
            playerScore = parsedBody;
            return true;
        }
        playerScore = null;
        return false;
    }
    
    public static bool TryExtractShopItemId(IHttpListenerRequestWrapper req, out string? itemId)
    {
        string requestBody= ExtractRequestBody(req);
        var itemRequest = System.Text.Json.JsonSerializer.Deserialize<PurchaseItemRequest>(requestBody);
        if (itemRequest != null)
        {
            itemId = itemRequest.ItemId;
            return true;
        }
        itemId = null;
        return false;
    }
}