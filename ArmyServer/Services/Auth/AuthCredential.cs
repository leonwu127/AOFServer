namespace ArmyServer.Services.Auth
{
    public class AuthCredential
    {
        public string Id { get; set; }
        public string Token { get; set; }

        public AuthCredential(string id, string token)
        {
            Id = id;
            Token = token;
        }
    }
}
