using ArmyServer.Models;

namespace ArmyServer.Services.Friends;

public interface IFriendsService
{
    List<Friend>? GetFriends(string playerId);
    void AddFriend(string playerId, Friend newFriend);
    bool RemoveFriend(string playerId, string friendId);
}