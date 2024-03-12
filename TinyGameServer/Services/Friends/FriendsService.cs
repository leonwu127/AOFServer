using TinyGameServer.Data;
using TinyGameServer.Excetions;
using TinyGameServer.Models;

namespace TinyGameServer.Services.Friends;

public class FriendsService : IFriendsService
{
    private readonly IDataRepository<string, List<Friend>> _friendsData;
    
    public FriendsService(IDataRepository<string, List<Friend>> friendsData)
    {
        _friendsData = friendsData;
    }
    
    public List<Friend>? GetFriends(string playerId)
    {
        return _friendsData.Get(playerId);
    }

    public void AddFriend(string playerId, Friend newFriend)
    {
        if (!_friendsData.Exists(playerId))
        {
            _friendsData.Add(playerId, new List<Friend> {newFriend});
        }
        else
        {
            List<Friend> friends = _friendsData.Get(playerId)!;
            if (friends.All(f => f.Id != newFriend.Id))
            {
                friends.Add(newFriend);
                _friendsData.Set(playerId,friends);
                return;
            }
            throw new AddExistingFriendException();
        }
    }

    public bool RemoveFriend(string playerId, string friendId)
    {
        List<Friend> friends = _friendsData.Get(playerId);
        if (friends == null) return false;
        Friend friendToRemove = friends.FirstOrDefault(f => f.Id == friendId);
        if (friendToRemove == null) return false;
        friends.Remove(friendToRemove);
        _friendsData.Set(playerId,friends);
        return true;
    }
}