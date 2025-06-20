using Microsoft.Extensions.Options;
using Movies.Common;
using Movies.Models;
using Movies.Models.ViewModels;

namespace Movies.Services
{
    public class UserService : IUserService
    {
        private readonly MoviesContext _context;
        private readonly AppSettings _appSettings;
        public UserService(MoviesContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;

        }
        public Task<UserResponse> Auth(AuthViewModel authModel)
        {

            UserResponse userResponse = new UserResponse();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(authModel.Password, _appSettings.Secret);
            throw new NotImplementedException();
        }
    }
}
