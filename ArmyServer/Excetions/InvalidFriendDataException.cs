namespace ArmyServer.Excetions;

public class InvalidFriendDataException : Exception
{
    public InvalidFriendDataException() : base("Invalid friend data")
    {
    }
}