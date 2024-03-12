using System.Net;
using System.Text.Json;
using TinyGameServer.Models.Response;
using TinyGameServer.Services.Shop;
using TinyGameServer.Utilities.HttpListenserWrapper;
using static TinyGameServer.Utilities.TokenUtility;
using static TinyGameServer.HttpServer;
using static TinyGameServer.Utilities.HttpUtilities.HttpUtility;

namespace TinyGameServer.Controllers
{
    public class ShopController
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public void GetShopItems(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!TokenIsValid(req, out string playerId))
            {
                SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized);
                return;
            }
            var shopItemsJson = JsonSerializer.Serialize(new ShopResponse(_shopService.GetAllShopItems()));
            
            SendResponse(resp, shopItemsJson, HttpStatusCode.OK);
        }

        public void PurchaseItem(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!TokenIsValid(req, out string playerId))
            {
                SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized);
                return;
            }

            if (TryExtractShopItemId(req, out string? itemId))
            {
                if (_shopService.PurchaseShopItem(playerId, itemId))
                {
                    SendResponse(resp, String.Empty, HttpStatusCode.OK);
                } 
                else
                {
                    SendError(resp, "Purchase failed.", HttpStatusCode.BadRequest);
                }
            }
            else
            {
                SendError(resp, "Failed to parse shop item.", HttpStatusCode.BadRequest);
            }
        }
    }
}