using ArmyServer.Data;
using ArmyServer.Models;
using ArmyServer.Services.Auth;
using System.Net;
using System.Numerics;

namespace ArmyServer.Services.Auth.Provider
{
    public interface IAuthentication
    {
        bool Authenticate(AuthCredential authCredential, out Player player);
    }
}
