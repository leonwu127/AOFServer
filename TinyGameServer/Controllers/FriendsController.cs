using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TinyGameServer.Excetions;
using TinyGameServer.Models;
using TinyGameServer.Services.Friends;
using TinyGameServer.Utilities.HttpListenserWrapper;
using static TinyGameServer.HttpServer;
using static TinyGameServer.Utilities.TokenUtility;
using static TinyGameServer.Utilities.HttpUtilities.HttpUtility;

namespace TinyGameServer.Controllers
{
    
    [ApiController]
    [Route("api/friends")]
    public class FriendsController
    {
        private readonly IFriendsService _friendsService;
        
        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }
        
        [HttpGet]
        public ActionResult<FriendResponse> GetFriends()
        {
            // validate token

            // if (!TokenIsValid(req, out string playerId))
            // { SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized); return; }
            //
            // List<Friend> friendsList = _friendsService.GetFriends(playerId)!;
            //
            // var response = JsonSerializer.Serialize(new FriendResponse(friendsList));
            // SendResponse(resp, response, HttpStatusCode.OK);
        }

        public void AddFriend(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!TokenIsValid(req, out string playerId))
            { SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized); return; }

            Friend newFriend;
            try
            {
                if (!TryExtractFriend(req, out newFriend)) { throw new InvalidFriendDataException(); }
            }
            catch (Exception ex) 
            {
                SendError(resp, $"Failed to parse friend information: {ex.Message}", HttpStatusCode.BadRequest);
                return;
            }

            try
            {
                _friendsService.AddFriend(playerId, newFriend); 
            }
            catch (Exception ex)
            {
                SendError(resp, $"Failed to add friend: {ex.Message}", HttpStatusCode.InternalServerError);
                return;
            }

            SendResponse(resp, string.Empty, HttpStatusCode.OK);

        }

        public void RemoveFriend(IHttpListenerRequestWrapper req, IHttpListenerResponseWrapper resp)
        {
            if (!TokenIsValid(req, out string playerId))
            { SendError(resp, "Unauthorized request.", HttpStatusCode.Unauthorized); return; }

            Friend friendToRemove;
            try
            {
                if (!TryExtractFriend(req, out friendToRemove)) { throw new InvalidFriendDataException(); }
            }
            catch (Exception ex) 
            {
                SendError(resp, $"Failed to parse friend removal information: {ex.Message}", HttpStatusCode.BadRequest);
                return;
            }

            if (!_friendsService.RemoveFriend(playerId, friendToRemove.Id))
            {
                SendError(resp, "Could not remove friend. Friend might not exist.", HttpStatusCode.NotFound);
                return;
            }
            SendResponse(resp, string.Empty, HttpStatusCode.OK);
        }

    }
}
