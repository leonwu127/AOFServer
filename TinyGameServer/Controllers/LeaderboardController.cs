using System.Net;
using System.Text.Json;
using TinyGameServer.Models;
using TinyGameServer.Models.Response;
using TinyGameServer.Services.Leaderboard;
using TinyGameServer.Utilities.HttpListenserWrapper;
using static TinyGameServer.HttpServer;
using static TinyGameServer.Utilities.TokenUtility;
using static TinyGameServer.Utilities.HttpUtilities.HttpUtility;

namespace TinyGameServer.Controllers
{
    public class LeaderboardController
    {
        private ILeaderboardService _leaderboardService;
        
        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }
        
        // Return the top 100 playerScores
        public void GetTopPlayers(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!TokenIsValid(req, out string playerId))
            {
                SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized);
                return;
            }
            
            var topPlayers = new LeaderboardResponse(_leaderboardService.GetTopPlayers());
            SendResponse(resp, JsonSerializer.Serialize(topPlayers), HttpStatusCode.OK);
        }        
        
        public void AddOrUpdatePlayerScore(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!TokenIsValid(req, out string playerId))
            {
                SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized);
                return;
            }
            
            if (TryExtractPlayerScore(req, out PlayerScore? playerScore))
            {
                _leaderboardService.AddOrUpdatePlayerScore(playerScore.Id, playerScore!.Name, playerScore.Score);
                SendResponse(resp, String.Empty, HttpStatusCode.OK);
            }
            else
            {
                SendError(resp, "Failed to parse player score.", HttpStatusCode.BadRequest);
            }
        }
    }
}
