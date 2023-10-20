namespace ArmyServer.Excetions;

public class AddExistingFriendException : Exception
{
    public AddExistingFriendException() : base("Friend already exists")
    {
    }
}