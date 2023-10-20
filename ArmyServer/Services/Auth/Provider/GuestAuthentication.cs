using ArmyServer.Data;
using ArmyServer.Models;
using ArmyServer.Utilities;

namespace ArmyServer.Services.Auth.Provider
{

    public class GuestAuthentication : IAuthentication
    {
        private IDataRepository<string, Player> PlayersData;
        public GuestAuthentication(IDataRepository<string, Player> playersData)
        {
            PlayersData = playersData;
        }
        
        public Player Authenticate()
        {
            // Generate a new player id, UUID
            Player newPlayer = new Player($"Guest_{Guid.NewGuid().ToString()}");
            PlayersData.Add(newPlayer.Id, newPlayer);

            return newPlayer;
        }

        public bool Authenticate(AuthCredential authCredential, out Player player)
        {
            player = null;
            if (authCredential == null)
            {
                return false;
            }
            var playerId = authCredential.Id;
            var token = authCredential.Token;
            if (string.IsNullOrEmpty(playerId) || string.IsNullOrEmpty(token))
            {
                return false;
            }
            if (!TokenUtility.IsTokenValid(token, playerId))
            {
                return false;
            }
            player = PlayersData.Get(playerId);
            if (player == null)
            {
                return false;
            }
            return true;
        }
    }
}
