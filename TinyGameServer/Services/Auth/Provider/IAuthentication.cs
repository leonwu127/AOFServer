using TinyGameServer.Data;
using TinyGameServer.Models;
using TinyGameServer.Services.Auth;
using System.Net;
using System.Numerics;

namespace TinyGameServer.Services.Auth.Provider
{
    public interface IAuthentication
    {
        bool Authenticate(AuthCredential authCredential, out Player player);
    }
}
