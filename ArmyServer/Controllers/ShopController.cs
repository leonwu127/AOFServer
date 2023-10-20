using System.Net;
using System.Text.Json;
using ArmyServer.Models.Response;
using ArmyServer.Services.Shop;
using ArmyServer.Utilities.HttpListenserWrapper;
using static ArmyServer.Utilities.TokenUtility;
using static ArmyServer.HttpServer;
using static ArmyServer.Utilities.HttpUtilities.HttpUtility;

namespace ArmyServer.Controllers
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

            var requestBody = DeserializeRequestBody(req); 
            string itemId = requestBody["itemId"];

            bool purchaseSuccessful = _shopService.PurchaseShopItem(playerId, itemId);
            if (purchaseSuccessful)
            {
                SendResponse(resp, String.Empty, HttpStatusCode.OK);
            }
            else
            {
                SendError(resp, "Purchase failed.", HttpStatusCode.BadRequest);
            }
        }
    }
}