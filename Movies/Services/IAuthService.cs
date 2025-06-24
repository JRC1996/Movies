using Movies.Common;
using Movies.Models.ViewModels;

namespace Movies.Services
{
    public interface IAuthService
    {
        UserResponse Auth(AuthViewModel model);
       
       
    }
}
