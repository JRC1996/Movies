namespace Movies.Services
{
    public interface ITokenBlackListService
    {

        Task<bool> IsTokenBlacklistedAsync(string tokenId);

        Task AddToBlacklistAsync(string tokenId, DateTime expiration);
      
    }
}
