using ArmyServer.Utilities;
using ArmyServer.Models;
using System.Net;
using System.Text.Json;
using ArmyServer.Data;
using ArmyServer.Models.Response;
using ArmyServer.Services.Auth;
using ArmyServer.Services.Auth.Provider;
using ArmyServer.Utilities.HttpListenserWrapper;

using static ArmyServer.HttpServer;
using static ArmyServer.Utilities.HttpUtilities.HttpUtility;

namespace ArmyServer.Controllers
{
    public class AuthenticationController
    {
        private readonly IDataRepository<string, Player> _playersData;

        
        public AuthenticationController(IDataRepository<string, Player> playersData)
        {
            _playersData = playersData;
        }
        
        private static readonly HashSet<GameTitle> SupportedGameTitles = new HashSet<GameTitle>
        {
            GameTitle.ArmyOfTactics
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
