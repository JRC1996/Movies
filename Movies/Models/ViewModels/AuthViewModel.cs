using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Movies.Models.ViewModels
{
    public class AuthViewModel
    {
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }
    }
}
