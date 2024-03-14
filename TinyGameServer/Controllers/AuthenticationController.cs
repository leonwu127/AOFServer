using TinyGameServer.Utilities;
using TinyGameServer.Models;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TinyGameServer.Data;
using TinyGameServer.Models.Response;
using TinyGameServer.Services.Auth;
using TinyGameServer.Services.Auth.Provider;
using TinyGameServer.Utilities.HttpListenserWrapper;

using static TinyGameServer.HttpServer;
using static TinyGameServer.Utilities.HttpUtilities.HttpUtility;

namespace TinyGameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IDataRepository<string, Player> _playersData;

        
        public AuthenticationController(IDataRepository<string, Player> playersData)
        {
            _playersData = playersData;
        }
        
        private static readonly HashSet<GameTitle> SupportedGameTitles = new HashSet<GameTitle>
        {
            GameTitle.TinyGame
        };

        private static readonly HashSet<Platform> SupportedPlatforms = new HashSet<Platform>
        {
            Platform.iOS,
            Platform.Android
        };
        
        
        public void GuestRegister(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!ValidateRequest(req, resp, out string msg))
            { SendError(resp, msg, HttpStatusCode.BadRequest); return; }

            GuestAuthentication authenticator = new GuestAuthentication(_playersData);
            var player = authenticator.Authenticate();
            var serverToken = TokenUtility.GenerateToken(player.Id);
            var authResponseJson = JsonSerializer.Serialize(new AuthResponse(player.Id, serverToken));
            SendResponse(resp, authResponseJson, HttpStatusCode.OK);
        }

        public void Login(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!ValidateRequest(req, resp, out string msg))
            { SendError(resp, msg, HttpStatusCode.BadRequest); return; }

            if (!TryExtractAuthCredential(req, out string provider, out AuthCredential authCredential, out msg))
            { SendError(resp, msg, HttpStatusCode.BadRequest); return; }

            IAuthentication authenticator;
            switch (provider)
            {
                case "GameCenter":
                case "Guest":
                    authenticator = new GuestAuthentication(_playersData); break;
                default:
                    SendError(resp, "Invalid authentication provider.", HttpStatusCode.BadRequest); return;
            }

            if (!authenticator.Authenticate(authCredential, out Player player)) 
            { SendError(resp, "Login failed.", HttpStatusCode.Unauthorized); return; }

            var serverToken = TokenUtility.GenerateToken(player.Id);
            var authResponseJson = JsonSerializer.Serialize(new AuthResponse(player.Id, serverToken));
            SendResponse(resp, authResponseJson, HttpStatusCode.OK);
        }

        private bool ValidateRequest(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp, out string message)
        {
            if  (!req.Headers.AllKeys.Contains("GameTitle") ||
                 !Enum.TryParse(req.Headers["GameTitle"], true, out GameTitle gameTitle) ||
                 !SupportedGameTitles.Contains(gameTitle))
            {
                message = "Invalid game title.";
                return false;
            }

            // Validate platform in the headers
            if (!req.Headers.AllKeys.Contains("platform") ||
                !Enum.TryParse(req.Headers["platform"], true, out Platform _platform) ||
                !SupportedPlatforms.Contains(_platform)){
                message = "Invalid platform.";
                return false;
            }
            message = string.Empty;
            return true;
        }
        
    }
}
