using System.Security.Cryptography;
using System.Text;
using ArmyServer.Utilities.HttpListenserWrapper;

namespace ArmyServer.Utilities
{
    public static class TokenUtility
    {
        // A secret key for token generation. In a real-world scenario, this should be stored securely.
        private static readonly string SecretKey = "YOUR_SECRET_KEY_HERE";

        // A dictionary to store tokens and associated player IDs for validation purposes.
        private static Dictionary<string, string> tokenStore = new();

        public static string GenerateToken(string playerId)
        {
            // Combine the player ID with the secret key and the current timestamp to generate a unique token.
            var tokenInput = $"{playerId}{SecretKey}{DateTime.UtcNow}";
            var token = ComputeSha256Hash(tokenInput);

            // Store the token and associated player ID for future validation.
            tokenStore[token] = playerId;

            return token;
        }

        public static bool ValidateToken(string token, out string playerId)
        {
            return tokenStore.TryGetValue(token, out playerId);
        }

        internal static bool IsTokenValid(string token, string playerId)
        {
            return tokenStore.TryGetValue(token, out var storedPlayerId) && storedPlayerId == playerId;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        public static bool TokenIsValid(IHttpListenerRequestWrapper req, out string playerId)
        {
            playerId = null;

            string authHeader = req.Headers["Authorization"];
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                return false;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            if (!ValidateToken(token, out playerId))
            {
                return false;
            }

            return true;
        }


    }
}
