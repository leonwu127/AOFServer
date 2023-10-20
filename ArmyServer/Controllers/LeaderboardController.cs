using System.Net;
using System.Text.Json;
using ArmyServer.Models.Response;
using ArmyServer.Services.Leaderboard;
using ArmyServer.Utilities.HttpListenserWrapper;
using static ArmyServer.HttpServer;
using static ArmyServer.Utilities.TokenUtility;
using static ArmyServer.Utilities.HttpUtilities.HttpUtility;

namespace ArmyServer.Controllers
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
            var body = DeserializeRequestBody(req);
            int newScore = int.Parse(body["newScore"]);
            string name = body["name"];
            string id = body["id"];
            if (id != playerId)
            {
                SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized);
                return;
            }
            _leaderboardService.AddOrUpdatePlayerScore(playerId, name, newScore);
            SendResponse(resp, String.Empty, HttpStatusCode.OK);
        }
    }
}
