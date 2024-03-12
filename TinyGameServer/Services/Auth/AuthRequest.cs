namespace TinyGameServer.Services.Auth
{
    public class AuthRequest
    {
        public Dictionary<string, AuthCredential> Provider { get; set; }
    }
}
