using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;

namespace Movies.Services
{
    public interface IAuthService
    {
       public Task <UserResponse> Auth(AuthViewModel model);
        
       public Task<UserResponse> ValidateRefreshToken(string refreshToken, string email);

        public Task<bool> RevokeRefreshToken (int userId);
    }
}
