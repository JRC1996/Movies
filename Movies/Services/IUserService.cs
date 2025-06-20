using Movies.Common;
using Movies.Models.ViewModels;

namespace Movies.Services
{
    public interface IUserService
    {
        Task<UserResponse> Auth(AuthViewModel authModel);
       
       
    }
}
