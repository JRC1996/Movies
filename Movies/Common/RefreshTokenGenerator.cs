using System.Security.Cryptography;

namespace Movies.Common
{
    public class RefreshTokenGenerator
    {
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
