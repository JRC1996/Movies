
using StackExchange.Redis;

namespace Movies.Services
{
    public class TokenBLService : ITokenBlackListService
    {

        private readonly IDatabase _redisDB;

        public TokenBLService(IConnectionMultiplexer redis)
        {
         
            _redisDB = redis.GetDatabase();
        }
        public async Task AddToBlacklistAsync(string tokenId, DateTime expiration)
        {
            var expiryTimeSpan = expiration - DateTime.UtcNow;
            if (expiryTimeSpan.TotalSeconds > 0)
            {
                await _redisDB.StringSetAsync($"BlackList:jwt:{tokenId}", "revoked ", expiryTimeSpan);
            }

           
        }

        public async Task<bool> IsTokenBlacklistedAsync(string tokenId)
        {
            return await _redisDB.KeyExistsAsync($"BlackList:jwt:{tokenId}");
        }
    }
}
