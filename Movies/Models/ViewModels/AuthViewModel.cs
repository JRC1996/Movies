using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class AuthViewModel
    {
        public string Email { get; set; }

        
        public string Password { get; set; }
    }
}
